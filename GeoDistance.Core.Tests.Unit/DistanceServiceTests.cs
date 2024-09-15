namespace GeoDistance.Core.Tests.Unit;

using GeoDistance.Core.Services;
using GeoDistance.Core.Dto;
using GeoDistance.Core.Exceptions;

using NSubstitute;

using Xunit;

public class DistanceServiceTests
{
    public DistanceServiceTests()
    {
        var httpClient = Substitute.For<HttpClient>();
        var geoCoordinateService = new GeoCoordinateService(httpClient);
        _distanceService = new DistanceService(geoCoordinateService);
    }

    private readonly IDistanceService _distanceService;
    
    private readonly IataModel _correctIataModel = new()
    {
        FirstAirport = "AMS",
        SecondAirport = "MSC"
    };
    
    private readonly IataModel _badIataModel = new()
    {
        FirstAirport = "",
        SecondAirport = ""
    };

    [Fact]
    public async Task GetDistance_WithBadIATAModel_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _distanceService.GetDistance(_badIataModel);
        });
    }
    
    [Fact]
    public async Task GetDistance_WithCorrectIATAModel_Returns()
    {
        var exception = 8675048.422827687;
        var actual = await _distanceService.GetDistance(_correctIataModel);
        
        Assert.Equal(exception, actual.Value);
    }
}