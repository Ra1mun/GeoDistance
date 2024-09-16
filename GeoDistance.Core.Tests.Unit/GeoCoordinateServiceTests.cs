using Microsoft.Extensions.Caching.Distributed;

namespace GeoDistance.Core.Tests.Unit;

using GeoDistance.Core.Dto;
using GeoDistance.Core.Exceptions;
using GeoDistance.Core.Services;

using Microsoft.Extensions.Caching.Memory;

using NSubstitute;

using Xunit;

public class GeoCoordinateServiceTests
{
    public GeoCoordinateServiceTests()
    {
        var httpClient = Substitute.For<HttpClient>();
        httpClient.BaseAddress = new Uri("https://places-dev.continent.ru/airports/");
        
        var memoryOptions = new MemoryCacheOptions();
        var memoryCache = new MemoryCache(memoryOptions);
        _geoCoordinateService = new GeoCoordinateService(httpClient, memoryCache);
    }

    private readonly IGeoCoordinateService _geoCoordinateService;

    private readonly string _correctAirportName = "AMS";

    private readonly string _badAirportName = "AQQ";

    [Fact]
    public async Task GetGeoCoordinate_WithBadAirportName_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidIataException>(async () =>
        {
            await _geoCoordinateService.GetGeoCoordinate(_badAirportName);
        });
    }
    
    [Fact]
    public async Task GetGeoCoordinate_WithCorrectAirportName_ThrowException()
    {
        var exception = new GeoCoordinate
        {
            Location = new Location
            {
                Longitude = 4.763385,
                Latitude = 52.309069
            }
        };
        
        var actual = await _geoCoordinateService.GetGeoCoordinate(_correctAirportName);
        
        Assert.Equal(exception, actual);
    }
}