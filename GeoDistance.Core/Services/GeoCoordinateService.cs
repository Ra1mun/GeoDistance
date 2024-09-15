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
    
    public async Task<GeoCoordinate?> GetGeoCoordinate(IataModel model)
    {
        var httpResponseMessage = await _httpClient.GetAsync(model.Name);
        var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
        if (!isSuccessStatusCode)
            throw new InvalidIataException("Failed to get GeoCoordinate", httpResponseMessage.StatusCode);

        var content = httpResponseMessage.Content;
        return await content.ReadFromJsonAsync<GeoCoordinate>()
               ?? throw new InvalidIataException("Failed to read data");
    }
}