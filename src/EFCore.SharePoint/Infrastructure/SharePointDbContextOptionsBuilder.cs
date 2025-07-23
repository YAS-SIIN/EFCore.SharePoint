// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.SharePoint.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

/// <summary>
///     Allows SharePoint specific configuration to be performed on <see cref="DbContextOptions" />.
/// </summary>
/// <remarks>
///     Instances of this class are returned from a call to
///     <see cref="O:SharePointDbContextOptionsExtensions.UseSharePoint" />
///     and it is not designed to be directly constructed in your application code.
/// </remarks>
public class SharePointDbContextOptionsBuilder
    : RelationalDbContextOptionsBuilder<SharePointDbContextOptionsBuilder, SharePointOptionsExtension>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SharePointDbContextOptionsBuilder" /> class.
    /// </summary>
    /// <param name="optionsBuilder">The options builder.</param>
    public SharePointDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        : base(optionsBuilder)
    {
    }

    /// <summary>
    ///     Sets the SharePoint site URL.
    /// </summary>
    /// <param name="siteUrl">The URL of the SharePoint site.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public virtual SharePointDbContextOptionsBuilder UseSiteUrl(string siteUrl)
        => WithOption(e => e.WithSiteUrl(siteUrl));

    /// <summary>
    ///     Sets the default SharePoint list name.
    /// </summary>
    /// <param name="listName">The name of the SharePoint list.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public virtual SharePointDbContextOptionsBuilder UseListName(string listName)
        => WithOption(e => e.WithListName(listName));

    /// <summary>
    ///     Configures whether to use client credentials for authentication.
    /// </summary>
    /// <param name="useClientCredentials">Whether to use client credentials.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public virtual SharePointDbContextOptionsBuilder UseClientCredentials(bool useClientCredentials = true)
        => WithOption(e => e.WithUseClientCredentials(useClientCredentials));
}
