// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.SharePoint.Update.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointModificationCommandBatchFactory : IModificationCommandBatchFactory
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointModificationCommandBatchFactory(ModificationCommandBatchFactoryDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    /// <summary>
    /// Dependencies for this service.
    /// </summary>
    protected virtual ModificationCommandBatchFactoryDependencies Dependencies { get; }

    /// <summary>
    /// Creates a new modification command batch.
    /// </summary>
    public virtual ModificationCommandBatch CreateBatch()
        => new SingularModificationCommandBatch(Dependencies);

    /// <summary>
    /// Creates a new modification command batch.
    /// </summary>
    public virtual ModificationCommandBatch Create()
        => CreateBatch();
}
