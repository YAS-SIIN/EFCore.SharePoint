// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http;
using System.Text.Json;

namespace Microsoft.EntityFrameworkCore.SharePoint.Storage.Internal;

/// <summary>
///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
///     the same compatibility standards as public APIs. It may be changed or removed without notice in
///     any release. You should only use it directly in your code with extreme caution and knowing that
///     doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public interface ISharePointClient
{
    /// <summary>
    ///     Gets the SharePoint site URL.
    /// </summary>
    string SiteUrl { get; }

    /// <summary>
    ///     Executes a query against SharePoint REST API.
    /// </summary>
    /// <param name="query">The REST API query.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The JSON response from SharePoint.</returns>
    Task<JsonDocument> ExecuteQueryAsync(string query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets items from a SharePoint list.
    /// </summary>
    /// <param name="listName">The name of the SharePoint list.</param>
    /// <param name="select">OData $select parameter.</param>
    /// <param name="filter">OData $filter parameter.</param>
    /// <param name="orderBy">OData $orderby parameter.</param>
    /// <param name="top">OData $top parameter.</param>
    /// <param name="skip">OData $skip parameter.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The list items as JSON.</returns>
    Task<JsonDocument> GetListItemsAsync(
        string listName,
        string? select = null,
        string? filter = null,
        string? orderBy = null,
        int? top = null,
        int? skip = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new item in a SharePoint list.
    /// </summary>
    /// <param name="listName">The name of the SharePoint list.</param>
    /// <param name="itemData">The item data as JSON.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The created item as JSON.</returns>
    Task<JsonDocument> CreateListItemAsync(
        string listName,
        JsonDocument itemData,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing item in a SharePoint list.
    /// </summary>
    /// <param name="listName">The name of the SharePoint list.</param>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <param name="itemData">The updated item data as JSON.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated item as JSON.</returns>
    Task<JsonDocument> UpdateListItemAsync(
        string listName,
        int itemId,
        JsonDocument itemData,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes an item from a SharePoint list.
    /// </summary>
    /// <param name="listName">The name of the SharePoint list.</param>
    /// <param name="itemId">The ID of the item to delete.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the delete operation.</returns>
    Task DeleteListItemAsync(
        string listName,
        int itemId,
        CancellationToken cancellationToken = default);
}

/// <summary>
///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
///     the same compatibility standards as public APIs. It may be changed or removed without notice in
///     any release. You should only use it directly in your code with extreme caution and knowing that
///     doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointClient : ISharePointClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _siteUrl;
    
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointClient(string siteUrl, HttpClient? httpClient = null)
    {
        _siteUrl = siteUrl ?? throw new ArgumentNullException(nameof(siteUrl));
        _httpClient = httpClient ?? new HttpClient();
        
        // Configure default headers for SharePoint REST API
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public string SiteUrl => _siteUrl;

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public async Task<JsonDocument> ExecuteQueryAsync(string query, CancellationToken cancellationToken = default)
    {
        var url = $"{_siteUrl.TrimEnd('/')}/_api/{query.TrimStart('/')}";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonDocument.Parse(content);
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public async Task<JsonDocument> GetListItemsAsync(
        string listName,
        string? select = null,
        string? filter = null,
        string? orderBy = null,
        int? top = null,
        int? skip = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>();
        
        if (!string.IsNullOrEmpty(select))
            queryParams.Add($"$select={Uri.EscapeDataString(select)}");
            
        if (!string.IsNullOrEmpty(filter))
            queryParams.Add($"$filter={Uri.EscapeDataString(filter)}");
            
        if (!string.IsNullOrEmpty(orderBy))
            queryParams.Add($"$orderby={Uri.EscapeDataString(orderBy)}");
            
        if (top.HasValue)
            queryParams.Add($"$top={top.Value}");
            
        if (skip.HasValue)
            queryParams.Add($"$skip={skip.Value}");

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
        var query = $"web/lists/getbytitle('{Uri.EscapeDataString(listName)}')/items{queryString}";
        
        return await ExecuteQueryAsync(query, cancellationToken);
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public async Task<JsonDocument> CreateListItemAsync(
        string listName,
        JsonDocument itemData,
        CancellationToken cancellationToken = default)
    {
        var url = $"{_siteUrl.TrimEnd('/')}/_api/web/lists/getbytitle('{Uri.EscapeDataString(listName)}')/items";
        
        var json = JsonSerializer.Serialize(itemData);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonDocument.Parse(responseContent);
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public async Task<JsonDocument> UpdateListItemAsync(
        string listName,
        int itemId,
        JsonDocument itemData,
        CancellationToken cancellationToken = default)
    {
        var url = $"{_siteUrl.TrimEnd('/')}/_api/web/lists/getbytitle('{Uri.EscapeDataString(listName)}')/items({itemId})";
        
        var json = JsonSerializer.Serialize(itemData);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        // SharePoint requires MERGE method and special headers for updates
        var request = new HttpRequestMessage(new HttpMethod("MERGE"), url)
        {
            Content = content
        };
        request.Headers.Add("IF-MATCH", "*");
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonDocument.Parse(responseContent);
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public async Task DeleteListItemAsync(
        string listName,
        int itemId,
        CancellationToken cancellationToken = default)
    {
        var url = $"{_siteUrl.TrimEnd('/')}/_api/web/lists/getbytitle('{Uri.EscapeDataString(listName)}')/items({itemId})";
        
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Add("IF-MATCH", "*");
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
