// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.EntityFrameworkCore.SharePoint.Examples;

/// <summary>
/// Example demonstrating basic usage of the SharePoint provider for Entity Framework Core.
/// </summary>
public class BasicUsageExample
{
    /// <summary>
    /// Example SharePoint context that maps to SharePoint lists.
    /// </summary>
    public class SharePointContext : DbContext
    {
        public SharePointContext(DbContextOptions<SharePointContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure SharePoint connection
                optionsBuilder.UseSharePoint("https://yourtenant.sharepoint.com/sites/yoursite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity mappings to SharePoint lists
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees"); // SharePoint list name
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name).HasColumnName("Title").IsRequired();
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.HireDate).HasColumnName("HireDate");
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentId");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id).HasColumnName("ID");
                entity.Property(d => d.Name).HasColumnName("Title").IsRequired();
            });
        }
    }

    /// <summary>
    /// Employee entity mapped to SharePoint list.
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime? HireDate { get; set; }
        public int? DepartmentId { get; set; }
    }

    /// <summary>
    /// Department entity mapped to SharePoint list.
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Example showing dependency injection setup.
    /// </summary>
    public static void ConfigureServices(IServiceCollection services)
    {
        // Method 1: Using AddSharePoint extension
        services.AddSharePoint<SharePointContext>(
            siteUrl: "https://yourtenant.sharepoint.com/sites/yoursite",
            sharePointOptions => 
            {
                sharePointOptions.UseClientCredentials(true);
                sharePointOptions.UseListName("Employees");
            });

        // Method 2: Using AddDbContext with UseSharePoint
        services.AddDbContext<SharePointContext>(options =>
        {
            options.UseSharePoint("https://yourtenant.sharepoint.com/sites/yoursite", sp =>
            {
                sp.UseSiteUrl("https://yourtenant.sharepoint.com/sites/yoursite");
                sp.UseListName("MyCustomList");
                sp.UseClientCredentials(true);
            });
        });
    }

    /// <summary>
    /// Example showing basic CRUD operations.
    /// </summary>
    public static async Task ExampleOperations()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSharePoint<SharePointContext>("https://yourtenant.sharepoint.com/sites/yoursite")
            .BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<SharePointContext>();

        try
        {
            // CREATE - Add a new employee
            var newEmployee = new Employee
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                HireDate = DateTime.Now,
                DepartmentId = 1
            };
            
            context.Employees.Add(newEmployee);
            await context.SaveChangesAsync();
            Console.WriteLine($"Created employee: {newEmployee.Name}");

            // READ - Query employees
            var employees = await context.Employees
                .Where(e => e.HireDate > DateTime.Now.AddMonths(-6))
                .OrderBy(e => e.Name)
                .ToListAsync();
            
            Console.WriteLine($"Found {employees.Count} recent hires");

            // UPDATE - Modify an employee
            var employee = await context.Employees
                .FirstOrDefaultAsync(e => e.Name == "John Doe");
            
            if (employee != null)
            {
                employee.Email = "john.doe@company.com";
                await context.SaveChangesAsync();
                Console.WriteLine($"Updated employee email: {employee.Email}");
            }

            // DELETE - Remove an employee
            if (employee != null)
            {
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
                Console.WriteLine("Deleted employee");
            }

            // Advanced queries
            var departmentEmployeeCounts = await context.Employees
                .GroupBy(e => e.DepartmentId)
                .Select(g => new { DepartmentId = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var item in departmentEmployeeCounts)
            {
                Console.WriteLine($"Department {item.DepartmentId}: {item.Count} employees");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Example showing configuration with data annotations.
    /// </summary>
    [Table("CustomEmployees")]
    public class AnnotatedEmployee
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("Title")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Column("Email")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Column("HireDate")]
        public DateTime? HireDate { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("Salary")]
        public decimal? Salary { get; set; }
    }
}
