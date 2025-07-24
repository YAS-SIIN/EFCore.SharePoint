// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Microsoft.EntityFrameworkCore.SharePoint.Scaffolding.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointCodeGenerator : IProviderConfigurationCodeGenerator
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual MethodCallCodeFragment GenerateUseProvider(
        string connectionString,
        MethodCallCodeFragment? providerOptions = null)
    {
        return new MethodCallCodeFragment(
            "UseSharePoint",
            providerOptions == null
                ? new object[] { connectionString }
                : new object[] { connectionString, new NestedClosureCodeFragment("x", providerOptions) });
    }

    /// <summary>
    /// Generates the provider options.
    /// </summary>
    public virtual MethodCallCodeFragment GenerateProviderOptions()
        => new("UseSharePoint");

    /// <summary>
    /// Generates the context options.
    /// </summary>
    public virtual MethodCallCodeFragment GenerateContextOptions()
        => new("UseSharePoint");
}
