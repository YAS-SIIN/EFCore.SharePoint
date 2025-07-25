﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SharePoint.Storage.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointExecutionStrategyFactory : IExecutionStrategyFactory
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointExecutionStrategyFactory(ExecutionStrategyDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    /// <summary>
    /// Dependencies for this service.
    /// </summary>
    protected virtual ExecutionStrategyDependencies Dependencies { get; }

    /// <summary>
    /// Creates an execution strategy.
    /// </summary>
    public virtual IExecutionStrategy Create()
        => new NonRetryingExecutionStrategy(Dependencies);
}
