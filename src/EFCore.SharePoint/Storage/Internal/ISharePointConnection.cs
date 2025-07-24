﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Microsoft.EntityFrameworkCore.SharePoint.Storage.Internal;

/// <summary>
///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
///     the same compatibility standards as public APIs. It may be changed or removed without notice in
///     any release. You should only use it directly in your code with extreme caution and knowing that
///     doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
/// <remarks>
///     The service lifetime is <see cref="ServiceLifetime.Scoped" />. This means that each
///     <see cref="DbContext" /> instance will use its own instance of this service.
///     The implementation may depend on other services registered with any lifetime.
///     The implementation does not need to be thread-safe.
/// </remarks>
public interface ISharePointConnection : IRelationalConnection
{
    /// <summary>
    ///     The SharePoint site URL.
    /// </summary>
    string? SiteUrl { get; }

    /// <summary>
    ///     Gets the HTTP client for SharePoint REST API calls.
    /// </summary>
    HttpClient HttpClient { get; }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    ISharePointConnection CreateAdminConnection();

    /// <summary>
    ///     Indicates whether the connection supports multiple concurrent SharePoint requests.
    /// </summary>
    bool IsMultipleActiveRequestsEnabled { get; }
}
