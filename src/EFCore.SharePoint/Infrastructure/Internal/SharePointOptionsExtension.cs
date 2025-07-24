// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Microsoft.EntityFrameworkCore.SharePoint.Infrastructure.Internal;

/// <summary>
///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
///     the same compatibility standards as public APIs. It may be changed or removed without notice in
///     any release. You should only use it directly in your code with extreme caution and knowing that
///     doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointOptionsExtension : RelationalOptionsExtension, IDbContextOptionsExtension
{
    private DbContextOptionsExtensionInfo? _info;
    private string? _siteUrl;
    private string? _listName;
    private bool _useClientCredentials;
    
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointOptionsExtension()
    {
    }
    
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    protected SharePointOptionsExtension(SharePointOptionsExtension copyFrom)
        : base(copyFrom)
    {
        _siteUrl = copyFrom._siteUrl;
        _listName = copyFrom._listName;
        _useClientCredentials = copyFrom._useClientCredentials;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public override DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this);

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    protected override RelationalOptionsExtension Clone()
        => new SharePointOptionsExtension(this);

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual string? SiteUrl => _siteUrl;

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual string? ListName => _listName;

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual bool UseClientCredentials => _useClientCredentials;

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual SharePointOptionsExtension WithSiteUrl(string? siteUrl)
    {
        var clone = (SharePointOptionsExtension)Clone();
        clone._siteUrl = siteUrl;
        return clone;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual SharePointOptionsExtension WithListName(string? listName)
    {
        var clone = (SharePointOptionsExtension)Clone();
        clone._listName = listName;
        return clone;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual SharePointOptionsExtension WithUseClientCredentials(bool useClientCredentials)
    {
        var clone = (SharePointOptionsExtension)Clone();
        clone._useClientCredentials = useClientCredentials;
        return clone;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public override void ApplyServices(IServiceCollection services)
    {
        services.AddEntityFrameworkSharePoint();
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public override void Validate(IDbContextOptions options)
    {
        base.Validate(options);
        
        if (string.IsNullOrWhiteSpace(_siteUrl))
        {
            throw new ArgumentException("SharePoint site URL is required.", nameof(_siteUrl));
        }
    }

    private sealed class ExtensionInfo(IDbContextOptionsExtension extension) : RelationalExtensionInfo(extension)
    {
        private string? _logFragment;

        private new SharePointOptionsExtension Extension
            => (SharePointOptionsExtension)base.Extension;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => other is ExtensionInfo otherInfo
                && Extension.SiteUrl == otherInfo.Extension.SiteUrl
                && Extension.ListName == otherInfo.Extension.ListName
                && Extension.UseClientCredentials == otherInfo.Extension.UseClientCredentials;

        public override string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();
                    builder.Append(base.LogFragment);
                    
                    if (!string.IsNullOrEmpty(Extension.SiteUrl))
                    {
                        builder.Append("SiteUrl=").Append(Extension.SiteUrl).Append(' ');
                    }
                    
                    if (!string.IsNullOrEmpty(Extension.ListName))
                    {
                        builder.Append("ListName=").Append(Extension.ListName).Append(' ');
                    }
                    
                    if (Extension.UseClientCredentials)
                    {
                        builder.Append("UseClientCredentials=").Append(Extension.UseClientCredentials).Append(' ');
                    }

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["SharePoint:SiteUrl"] = Extension.SiteUrl ?? "(null)";
            debugInfo["SharePoint:ListName"] = Extension.ListName ?? "(null)";
            debugInfo["SharePoint:UseClientCredentials"] = Extension.UseClientCredentials.ToString();
        }
    }
}
