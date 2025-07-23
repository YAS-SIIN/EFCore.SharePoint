// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SharePoint.Infrastructure.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

/// <summary>
///     SharePoint specific extension methods for <see cref="DbContextOptionsBuilder" />.
/// </summary>
/// <remarks>
///     See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
///     <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
///     for more information and examples.
/// </remarks>
public static class SharePointDbContextOptionsBuilderExtensions
{
    /// <summary>
    ///     Configures the context to connect to a SharePoint site, but without initially setting any
    ///     site URL or connection parameters.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The site URL must be set before the <see cref="DbContext" /> is used to connect
    ///         to SharePoint.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
    ///         <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
    ///         for more information and examples.
    ///     </para>
    /// </remarks>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="sharePointOptionsAction">An optional action to allow additional SharePoint specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder UseSharePoint(
        this DbContextOptionsBuilder optionsBuilder,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction = null)
    {
        var extension = GetOrCreateExtension<SharePointOptionsExtension>(optionsBuilder);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        return ApplyConfiguration(optionsBuilder, sharePointOptionsAction);
    }

    /// <summary>
    ///     Configures the context to connect to a SharePoint site.
    /// </summary>
    /// <remarks>
    ///     See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
    ///     <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
    ///     for more information and examples.
    /// </remarks>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="siteUrl">The URL of the SharePoint site to connect to.</param>
    /// <param name="sharePointOptionsAction">An optional action to allow additional SharePoint specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder UseSharePoint(
        this DbContextOptionsBuilder optionsBuilder,
        string? siteUrl,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction = null)
    {
        var extension = GetOrCreateExtension<SharePointOptionsExtension>(optionsBuilder);
        extension = extension.WithSiteUrl(siteUrl);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        return ApplyConfiguration(optionsBuilder, sharePointOptionsAction);
    }

    /// <summary>
    ///     Configures the context to connect to a SharePoint site with a specific list.
    /// </summary>
    /// <remarks>
    ///     See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
    ///     <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
    ///     for more information and examples.
    /// </remarks>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="siteUrl">The URL of the SharePoint site to connect to.</param>
    /// <param name="listName">The name of the SharePoint list to use as the primary data source.</param>
    /// <param name="sharePointOptionsAction">An optional action to allow additional SharePoint specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder UseSharePoint(
        this DbContextOptionsBuilder optionsBuilder,
        string? siteUrl,
        string? listName,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction = null)
    {
        var extension = GetOrCreateExtension<SharePointOptionsExtension>(optionsBuilder);
        extension = extension
            .WithSiteUrl(siteUrl)
            .WithListName(listName);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
        return ApplyConfiguration(optionsBuilder, sharePointOptionsAction);
    }

    /// <summary>
    ///     Configures the context to connect to a SharePoint site, but without initially setting any
    ///     site URL or connection parameters.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The site URL must be set before the <see cref="DbContext" /> is used to connect
    ///         to SharePoint.
    ///     </para>
    ///     <para>
    ///         See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
    ///         <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
    ///         for more information and examples.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TContext">The type of context to be configured.</typeparam>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="sharePointOptionsAction">An optional action to allow additional SharePoint specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder<TContext> UseSharePoint<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSharePoint(
            (DbContextOptionsBuilder)optionsBuilder, sharePointOptionsAction);

    /// <summary>
    ///     Configures the context to connect to a SharePoint site.
    /// </summary>
    /// <remarks>
    ///     See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
    ///     <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
    ///     for more information and examples.
    /// </remarks>
    /// <typeparam name="TContext">The type of context to be configured.</typeparam>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="siteUrl">The URL of the SharePoint site to connect to.</param>
    /// <param name="sharePointOptionsAction">An optional action to allow additional SharePoint specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder<TContext> UseSharePoint<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        string? siteUrl,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSharePoint(
            (DbContextOptionsBuilder)optionsBuilder, siteUrl, sharePointOptionsAction);

    /// <summary>
    ///     Configures the context to connect to a SharePoint site with a specific list.
    /// </summary>
    /// <remarks>
    ///     See <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see>, and
    ///     <see href="https://aka.ms/efcore-docs-sharepoint">Accessing SharePoint lists with EF Core</see>
    ///     for more information and examples.
    /// </remarks>
    /// <typeparam name="TContext">The type of context to be configured.</typeparam>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    /// <param name="siteUrl">The URL of the SharePoint site to connect to.</param>
    /// <param name="listName">The name of the SharePoint list to use as the primary data source.</param>
    /// <param name="sharePointOptionsAction">An optional action to allow additional SharePoint specific configuration.</param>
    /// <returns>The options builder so that further configuration can be chained.</returns>
    public static DbContextOptionsBuilder<TContext> UseSharePoint<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        string? siteUrl,
        string? listName,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction = null)
        where TContext : DbContext
        => (DbContextOptionsBuilder<TContext>)UseSharePoint(
            (DbContextOptionsBuilder)optionsBuilder, siteUrl, listName, sharePointOptionsAction);

    private static TExtension GetOrCreateExtension<TExtension>(DbContextOptionsBuilder optionsBuilder)
        where TExtension : class, IDbContextOptionsExtension, new()
        => optionsBuilder.Options.FindExtension<TExtension>() ?? new TExtension();

    private static DbContextOptionsBuilder ApplyConfiguration(
        DbContextOptionsBuilder optionsBuilder,
        Action<SharePointDbContextOptionsBuilder>? sharePointOptionsAction)
    {
        ConfigureWarnings(optionsBuilder);

        sharePointOptionsAction?.Invoke(new SharePointDbContextOptionsBuilder(optionsBuilder));

        var extension = GetOrCreateExtension<SharePointOptionsExtension>(optionsBuilder);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

    private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
    {
        var coreOptionsExtension
            = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()
            ?? new CoreOptionsExtension();

        coreOptionsExtension = RelationalOptionsExtension.WithDefaultWarningConfiguration(coreOptionsExtension);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
    }
}
