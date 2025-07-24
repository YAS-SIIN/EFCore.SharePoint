# EFCore.SharePoint

Entity Framework Core provider for SharePoint lists that enables EF Core to treat SharePoint lists as relational tables.

## Features

- **SharePoint Online Support**: Connect to SharePoint Online sites using modern authentication
- **List-as-Table Mapping**: Map SharePoint lists to Entity Framework entities
- **CRUD Operations**: Support for Create, Read, Update, Delete operations on SharePoint list items
- **Query Translation**: Translate LINQ queries to SharePoint REST API calls
- **Authentication Options**: Support for various authentication methods, including client credentials

## Usage

### Basic Setup

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class SharePointContext : DbContext
{
    public SharePointContext(DbContextOptions<SharePointContext> options)
        : base(options)
    {
    }
    
    public DbSet<Employee> Employees { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSharePoint("https://yourtenant.sharepoint.com/sites/yoursite");
        }
    }
}

public class Employee
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Email { get; set; }
    public DateTime? HireDate { get; set; }
}
```

### Dependency Injection

```csharp
services.AddSharePoint<SharePointContext>(
    siteUrl: "https://yourtenant.sharepoint.com/sites/yoursite",
    sharePointOptions => 
    {
        sharePointOptions.UseClientCredentials(true);
        sharePointOptions.UseListName("Employees");
    });
```

### Configuration with Options

```csharp
services.AddDbContext<SharePointContext>(options =>
{
    options.UseSharePoint("https://yourtenant.sharepoint.com/sites/yoursite", sp =>
    {
        sp.UseSiteUrl("https://yourtenant.sharepoint.com/sites/yoursite");
        sp.UseListName("MyCustomList");
        sp.UseClientCredentials(true);
    });
});
```

### Querying Data

```csharp
using (var context = new SharePointContext())
{
    // Basic queries
    var employees = await context.Employees.ToListAsync();
    
    // Filtered queries
    var recentHires = await context.Employees
        .Where(e => e.HireDate > DateTime.Now.AddMonths(-3))
        .ToListAsync();
    
    // Ordering
    var sortedEmployees = await context.Employees
        .OrderBy(e => e.Title)
        .ToListAsync();
}
```

### CRUD Operations

```csharp
using (var context = new SharePointContext())
{
    // Create
    var newEmployee = new Employee
    {
        Title = "John Doe",
        Email = "john.doe@example.com",
        HireDate = DateTime.Now
    };
    context.Employees.Add(newEmployee);
    await context.SaveChangesAsync();
    
    // Update
    var employee = await context.Employees.FirstAsync(e => e.Title == "John Doe");
    employee.Email = "john.doe@company.com";
    await context.SaveChangesAsync();
    
    // Delete
    context.Employees.Remove(employee);
    await context.SaveChangesAsync();
}
```

## Configuration Options

### SharePoint Site Configuration

- **SiteUrl**: The URL of the SharePoint site
- **ListName**: Default list name for entities (can be overridden per entity)
- **UseClientCredentials**: Enable client credentials authentication

### Entity Mapping

Map entities to specific SharePoint lists using data annotations or Fluent API:

```csharp
[Table("CustomListName")]
public class Employee
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }
    
    [Column("Title")]
    public string Name { get; set; }
    
    [Column("Email")]
    public string EmailAddress { get; set; }
}
```

Or using Fluent API:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>(entity =>
    {
        entity.ToTable("CustomListName");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.Name).HasColumnName("Title");
        entity.Property(e => e.EmailAddress).HasColumnName("Email");
    });
}
```

## Authentication

### Client Credentials Flow

For server-to-server authentication:

```csharp
optionsBuilder.UseSharePoint(siteUrl, sp =>
{
    sp.UseClientCredentials(true);
});
```

### User Authentication

For applications requiring user context:

```csharp
optionsBuilder.UseSharePoint(siteUrl, sp =>
{
    sp.UseClientCredentials(false); // Default
});
```

## Limitations

- **Schema Migrations**: SharePoint lists have limited schema modification capabilities
- **Complex Relationships**: Limited support for complex entity relationships
- **Transactions**: SharePoint doesn't support traditional database transactions
- **Performance**: REST API calls may be slower than direct database queries

## Supported Data Types

- String (Text, Note)
- Integer (Number)
- DateTime (Date and Time)
- Boolean (Yes/No)
- Decimal (Number with decimals)
- Guid (Lookup IDs)

## Advanced Features

### Custom List Operations

```csharp
public class CustomSharePointContext : SharePointContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Map different entities to different lists
        modelBuilder.Entity<Employee>().ToTable("Employees");
        modelBuilder.Entity<Department>().ToTable("Departments");
    }
}
```

### Query Optimization

The provider automatically translates LINQ queries to efficient SharePoint REST API calls with proper OData parameters:

- `Where` clauses become `$filter`
- `OrderBy` becomes `$orderby`  
- `Select` projections become `$select`
- `Take` becomes `$top`
- `Skip` becomes `$skip`

## Contributing

This provider follows the Entity Framework Core provider patterns and architecture. See the EF Core documentation for provider development guidelines.

## License

Licensed under the MIT License. See LICENSE file for details.
