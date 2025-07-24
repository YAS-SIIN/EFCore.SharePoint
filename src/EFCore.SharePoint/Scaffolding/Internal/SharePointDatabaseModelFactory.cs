// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data.Common;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Microsoft.EntityFrameworkCore.SharePoint.Scaffolding.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointDatabaseModelFactory : IDatabaseModelFactory
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
    {
        // For SharePoint, we'll create a basic database model
        // In a real implementation, this would connect to SharePoint and discover lists/columns
        var databaseModel = new DatabaseModel
        {
            DatabaseName = "SharePoint",
            DefaultSchema = null
        };

        return databaseModel;
    }

    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
    {
        // For SharePoint, we'll create a basic database model
        // In a real implementation, this would use the connection to discover SharePoint structure
        var databaseModel = new DatabaseModel
        {
            DatabaseName = "SharePoint",
            DefaultSchema = null
        };

        return databaseModel;
    }
}
