// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SharePoint.Query.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointSqlTranslatingExpressionVisitorFactory : IRelationalSqlTranslatingExpressionVisitorFactory
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointSqlTranslatingExpressionVisitorFactory(
        RelationalSqlTranslatingExpressionVisitorDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    /// <summary>
    /// Dependencies for this service.
    /// </summary>
    protected virtual RelationalSqlTranslatingExpressionVisitorDependencies Dependencies { get; }

    /// <summary>
    /// Creates a SQL translating expression visitor.
    /// </summary>
    public virtual RelationalSqlTranslatingExpressionVisitor Create(
        QueryCompilationContext queryCompilationContext,
        QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
        => new RelationalSqlTranslatingExpressionVisitor(
            Dependencies,
            queryCompilationContext,
            queryableMethodTranslatingExpressionVisitor);
}
