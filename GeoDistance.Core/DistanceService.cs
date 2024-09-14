namespace GeopositionDistace;

using System.Net.Http.Json;

public class DistanceService : IDistanceService
{
    private readonly HttpClient _httpClient;

    public DistanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double> GetDistance(string firstIATA, string secondIATA)
    {
        var firstPoint = await GetGeoPosition(firstIATA);
        var secondPoint = await GetGeoPosition(secondIATA);

        return await CalculateDistance(firstPoint.Location, secondPoint.Location);
    }

    public async Task<GeoPosition?> GetGeoPosition(string iata)
    {
        var httpResponseMessage = await _httpClient.GetAsync(iata);
        var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
        if (!isSuccessStatusCode)
            throw new InvalidOperationException(
                $"Failed to get geoposition, error number: {httpResponseMessage.StatusCode}");

        return await httpResponseMessage.Content.ReadFromJsonAsync<GeoPosition>();
    }

    private Task<double> CalculateDistance(Location point1, Location point2)
    {
        var d1 = point1.Latitude * (Math.PI / 180.0);
        var num1 = point1.Longitude * (Math.PI / 180.0);
        var d2 = point2.Latitude * (Math.PI / 180.0);
        var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                 Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return Task.FromResult(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
    }
}