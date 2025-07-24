// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SharePoint.Storage.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointDatabaseCreator : RelationalDatabaseCreator
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointDatabaseCreator(
        RelationalDatabaseCreatorDependencies dependencies)
        : base(dependencies)
    {
    }

    /// <summary>
    /// SharePoint lists are always available, so this always returns true.
    /// </summary>
    public override bool Exists()
        => true;

    /// <summary>
    /// SharePoint lists are always available, so this always returns true.
    /// </summary>
    public override Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(true);

    /// <summary>
    /// SharePoint lists don't require creation, so this does nothing.
    /// </summary>
    public override void Create()
    {
        // SharePoint lists don't require explicit creation
    }

    /// <summary>
    /// SharePoint lists don't require creation, so this does nothing.
    /// </summary>
    public override Task CreateAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    /// <summary>
    /// SharePoint lists cannot be deleted through EF Core, so this does nothing.
    /// </summary>
    public override void Delete()
    {
        // SharePoint lists cannot be deleted through EF Core
    }

    /// <summary>
    /// SharePoint lists cannot be deleted through EF Core, so this does nothing.
    /// </summary>
    public override Task DeleteAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    /// <summary>
    /// SharePoint lists are always available, so this always returns true.
    /// </summary>
    public override bool HasTables()
        => true;

    /// <summary>
    /// SharePoint lists are always available, so this always returns true.
    /// </summary>
    public override Task<bool> HasTablesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(true);
}
