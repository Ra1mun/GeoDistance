using System.Net.Http.Json;
using GeoDistance.Core.Dto;
using GeoDistance.Core.Exceptions;

namespace GeoDistance.Core.Services;

public class GeoCoordinateService : IGeoCoordinateService
{
    private readonly HttpClient _httpClient;

    public GeoCoordinateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<(GeoCoordinate, GeoCoordinate)> GetGeoCoordinates(IataModel model)
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