// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SharePoint.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.Storage.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.SharePoint.Query.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.Update.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.Migrations.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     EntityFrameworkCore.SharePoint extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class SharePointServiceCollectionExtensions
{
    /// <summary>
    ///     Registers the given Entity Framework <see cref="DbContext" /> as a service in the <see cref="IServiceCollection" />
    ///     and configures it to connect to a SharePoint site.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method is a shortcut for configuring a <see cref="DbContext" /> to use SharePoint. It does not support all options.
    ///         Use <see cref="O:EntityFrameworkServiceCollectionExtensions.AddDbContext" /> and related methods for full control of
    ///         this process.
    ///     </para>
    ///     <para>
    ///         Use this method when using dependency injection in your application, such as with ASP.NET Core.
    ///         For applications that don't use dependency injection, consider creating <see cref="DbContext" />
    ///         instances directly with its constructor. The <see cref="DbContext.OnConfiguring" /> method can then be
    ///         overridden to configure the SharePoint provider and site URL.
    ///     </para>
    ///     <para>
    ///         To configure the <see cref="DbContextOptions{TContext}" /> for the context, either override the
    ///         <see cref="DbContext.OnConfiguring" /> method in your derived context, or supply
    ///         an optional action to configure the <see cref="DbContextOptions" /> for the context.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-di">Using DbContext with dependency injection</see> for more information and examples.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
    ///         <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
    ///         for more information and examples.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TContext">The type of context to be registered.</typeparam>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="siteUrl">The URL of the SharePoint site to connect to.</param>
    /// <param name="sharePointOptionsAction">An optional action to allow additional SharePoint specific configuration.</param>
    /// <param name="optionsAction">An optional action to configure the <see cref="DbContextOptions" /> for the context.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddSharePoint<TContext>(
        this IServiceCollection serviceCollection,
        string? siteUrl,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction = null,
        Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        => serviceCollection.AddDbContext<TContext>(
            (_, options) =>
            {
                optionsAction?.Invoke(options);
                options.UseSharePoint(siteUrl, sharePointOptionsAction);
            });

    /// <summary>
    ///     <para>
    ///         Adds the services required by the Microsoft SharePoint database provider for Entity Framework
    ///         to an <see cref="IServiceCollection" />.
    ///     </para>
    ///     <para>
    ///         Warning: Do not call this method accidentally. It is much more likely you need
    ///         to call <see cref="AddSharePoint{TContext}" />.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     Calling this method is no longer necessary when building most applications, including those that
    ///     use dependency injection in ASP.NET or elsewhere.
    ///     It is only needed when building the internal service provider for use with
    ///     the <see cref="DbContextOptionsBuilder.UseInternalServiceProvider" /> method.
    ///     This is not recommend other than for some advanced scenarios.
    /// </remarks>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>
    ///     The same service collection so that multiple calls can be chained.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IServiceCollection AddEntityFrameworkSharePoint(this IServiceCollection serviceCollection)
    {
        new EntityFrameworkRelationalServicesBuilder(serviceCollection)
            .TryAdd<LoggingDefinitions, SharePointLoggingDefinitions>()
            .TryAdd<IDatabaseProvider, DatabaseProvider<SharePointOptionsExtension>>()
            .TryAdd<IRelationalTypeMappingSource, SharePointTypeMappingSource>()
            .TryAdd<ISqlGenerationHelper, SharePointSqlGenerationHelper>()
            .TryAdd<IProviderConventionSetBuilder, SharePointConventionSetBuilder>()
            .TryAdd<IUpdateSqlGenerator, SharePointUpdateSqlGenerator>()
            .TryAdd<IModificationCommandBatchFactory, SharePointModificationCommandBatchFactory>()
            .TryAdd<IModificationCommandFactory, SharePointModificationCommandFactory>()
            .TryAdd<IValueGeneratorSelector, SharePointValueGeneratorSelector>()
            .TryAdd<IRelationalConnection>(p => p.GetRequiredService<ISharePointConnection>())
            .TryAdd<IMigrationsSqlGenerator, SharePointMigrationsSqlGenerator>()
            .TryAdd<IRelationalDatabaseCreator, SharePointDatabaseCreator>()
            .TryAdd<IHistoryRepository, SharePointHistoryRepository>()
            .TryAdd<IExecutionStrategyFactory, SharePointExecutionStrategyFactory>()
            .TryAdd<IRelationalQueryStringFactory, SharePointQueryStringFactory>()
            .TryAdd<ICompiledQueryCacheKeyGenerator, SharePointCompiledQueryCacheKeyGenerator>()
            .TryAdd<IQueryCompilationContextFactory, SharePointQueryCompilationContextFactory>()
            .TryAdd<IMethodCallTranslatorProvider, SharePointMethodCallTranslatorProvider>()
            .TryAdd<IAggregateMethodCallTranslatorProvider, SharePointAggregateMethodCallTranslatorProvider>()
            .TryAdd<IMemberTranslatorProvider, SharePointMemberTranslatorProvider>()
            .TryAdd<IQuerySqlGeneratorFactory, SharePointQuerySqlGeneratorFactory>()
            .TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, SharePointSqlTranslatingExpressionVisitorFactory>()
            .TryAdd<ISqlExpressionFactory, SharePointSqlExpressionFactory>()
            .TryAdd<IQueryTranslationPostprocessorFactory, SharePointQueryTranslationPostprocessorFactory>()
            .TryAdd<IRelationalParameterBasedSqlProcessorFactory, SharePointParameterBasedSqlProcessorFactory>()
            .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, SharePointQueryableMethodTranslatingExpressionVisitorFactory>()
            .TryAddProviderSpecificServices(
                b => b
                    .TryAddScoped<ISharePointConnection, SharePointConnection>())
            .TryAddCoreServices();

        return serviceCollection;
    }
}
