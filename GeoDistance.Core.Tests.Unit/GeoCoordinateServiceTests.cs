namespace GeoDistance.Core.Tests.Unit;

using GeoDistance.Core.Dto;
using GeoDistance.Core.Exceptions;
using GeoDistance.Core.Services;

using NSubstitute;

using Xunit;

public class GeoCoordinateServiceTests
{
    public GeoCoordinateServiceTests()
    {
        var httpClient = Substitute.For<HttpClient>();
        _geoCoordinateService = new GeoCoordinateService(httpClient);
    }

    private readonly IGeoCoordinateService _geoCoordinateService;

    private readonly IataModel _correctIataModel = new()
    {
        Name = "AMS"
    };
    
    private readonly IataModel _badIataModel = new()
    {
        Name = ""
    };

    [Fact]
    public async Task GetGeoCoordinate_WithoutIATA_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _geoCoordinateService.GetGeoCoordinate(_badIataModel);
        });
    }
    
    [Fact]
    public async Task GetGeoCoordinate_WithCorrectIATA_ThrowException()
    {
        var exception = new GeoCoordinate
        {
            Location = new Location
            {
                Longitude = 4.763385,
                Latitude = 52.309069
            }
        };
        
        var actual = await _geoCoordinateService.GetGeoCoordinate(_correctIataModel);
        
        Assert.Equal(exception, actual);
    }
}