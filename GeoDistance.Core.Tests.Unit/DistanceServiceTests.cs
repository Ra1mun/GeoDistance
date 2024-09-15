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
    
    private readonly IataModel _firstIataModel = new()
    {
        Name = "AMS"
    };

    private readonly IataModel _secondIataModel = new()
    {
        Name = "MSC"
    };
    
    private readonly IataModel _badIataModel = new()
    {
        Name = ""
    };

    [Fact]
    public async Task GetDistance_WithBadIATAs_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidIataException>(async () =>
        {
            await _distanceService.GetDistance(_badIataModel, _badIataModel);
        });
    }
    
    [Fact]
    public async Task GetDistance_WithCorrectAndBadIATAs_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidIataException>(async () =>
        {
            await _distanceService.GetDistance(_firstIataModel, _badIataModel);
        });
    }
    
    [Fact]
    public async Task GetDistance_WithCorrectIATAs_Returns()
    {
        var exception = 8675048.422827687;
        var actual = await _distanceService.GetDistance(_firstIataModel, _badIataModel);
        
        Assert.Equal(exception, actual.Value);
    }
}