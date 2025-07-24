// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SharePoint.Query.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointParameterBasedSqlProcessorFactory(
        RelationalParameterBasedSqlProcessorDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    /// <summary>
    /// Dependencies for this service.
    /// </summary>
    protected virtual RelationalParameterBasedSqlProcessorDependencies Dependencies { get; }

    /// <summary>
    /// Creates a parameter-based SQL processor.
    /// </summary>
    public virtual RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
        => new RelationalParameterBasedSqlProcessor(
            Dependencies,
#pragma warning disable EF1001 // Internal EF Core API usage.
            new RelationalParameterBasedSqlProcessorParameters(useRelationalNulls, ParameterTranslationMode.MultipleParameters));
#pragma warning restore EF1001 // Internal EF Core API usage.

    /// <summary>
    /// Creates a parameter-based SQL processor.
    /// </summary>
    public virtual RelationalParameterBasedSqlProcessor Create(RelationalParameterBasedSqlProcessorParameters parameters)
        => new RelationalParameterBasedSqlProcessor(
            Dependencies,
            parameters);
}
