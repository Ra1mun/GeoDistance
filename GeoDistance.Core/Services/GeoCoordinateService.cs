using Microsoft.Extensions.Caching.Memory;

namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;
using GeoDistance.Core.Exceptions;

using System.Net.Http.Json;

public class GeoCoordinateService : IGeoCoordinateService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public GeoCoordinateService(HttpClient httpClient,
        IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }


    public async Task<(GeoCoordinate, GeoCoordinate)> GetTwoGeoCoordinates(IataModel model)
    {
        var firstCoordinate = await GetGeoCoordinate(model.FirstAirport);
        var secondCoordinate = await GetGeoCoordinate(model.SecondAirport);
        
        return (firstCoordinate, secondCoordinate);
    }

    public async Task<GeoCoordinate?> GetGeoCoordinate(string airport)
    {
        var httpResponseMessage = await _httpClient.GetAsync(airport);
        var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
        if (!isSuccessStatusCode)
            throw new InvalidIataException("Failed to get GeoCoordinate", httpResponseMessage.StatusCode);

        var content = httpResponseMessage.Content;
        return await content.ReadFromJsonAsync<GeoCoordinate>()
               ?? throw new InvalidIataException("Failed to read data");
    }
}