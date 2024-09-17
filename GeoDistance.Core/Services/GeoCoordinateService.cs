namespace GeoDistance.Core.Services;

using System.Net.Http.Json;

using GeoDistance.Core.Configuration;
using GeoDistance.Core.Dto;
using GeoDistance.Core.Exceptions;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

public class GeoCoordinateService : IGeoCoordinateService
{
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly GeoCoordinateServerConfiguration _serverOptions;

    public GeoCoordinateService(
        IOptions<CacheConfiguration> cacheOptions,
        IOptions<GeoCoordinateServerConfiguration> serverOptions,
        IHttpClientFactory httpClientFactory)
    {
        _serverOptions = serverOptions.Value;
        _httpClientFactory = httpClientFactory;
        _cacheEntryOptions = CreateCacheOptions(cacheOptions.Value);
        _memoryCache = CreateCache();
    }

    public async Task<GeoCoordinate> GetGeoCoordinateAsync(
        string airport,
        CancellationToken cancellationToken = default)
    {
        var key = airport.ToUpperInvariant();
        if (_memoryCache.TryGetValue<GeoCoordinate>(key, out var value))
            return value!;

        var coordinate = await GetGeoCoordinate(key, cancellationToken);
        _memoryCache.Set(key, coordinate, _cacheEntryOptions);
        return coordinate;
    }

    private static MemoryCache CreateCache()
    {
        var memoryCacheOptions = new MemoryCacheOptions();
        return new MemoryCache(memoryCacheOptions);
    }

    private static MemoryCacheEntryOptions CreateCacheOptions(CacheConfiguration cacheConfiguration)
    {
        return new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheConfiguration.AbsoluteExpirationInSeconds),
            SlidingExpiration = TimeSpan.FromSeconds(cacheConfiguration.SlidingExpirationInSeconds),
            Priority = CacheItemPriority.High,
        };
    }

    private async Task<GeoCoordinate> GetGeoCoordinate(string airport, CancellationToken cancellationToken = default)
    {
        var httpClient = CreateClient();
        var httpResponseMessage = await httpClient.GetAsync(airport, cancellationToken);
        var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
        if (!isSuccessStatusCode)
            throw new InvalidIataException("Failed to get GeoCoordinate", (int)httpResponseMessage.StatusCode);

        var content = httpResponseMessage.Content;
        return await content.ReadFromJsonAsync<GeoCoordinate>(cancellationToken: cancellationToken)
               ?? throw new InvalidIataException("Failed to read data");
    }

    private HttpClient CreateClient()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = _serverOptions.Url;
        if (string.IsNullOrEmpty(url))
        {
            throw new ConfigurationException("Missing url");
        }
        
        httpClient.BaseAddress = new Uri(url);
        return httpClient;
    }
}