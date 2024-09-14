using GeoDistance;


namespace GeopositionDistance.Tests;

public class DistanceServiceTests
{
    private readonly string FirstIATA = "AMS";
    private readonly string SecondIATA = "MSC";
    
    private readonly HttpClient _httpClient;

    public DistanceServiceTests(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [Fact]
    public async Task GetGeoposition_WithNull_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            IDistanceService distanceService = new DistanceService(_httpClient);
            await distanceService.GetGeoPosition(null);
        });
    }
    
    [Fact]
    public async Task GetGeoposition_WithCorrectIATA_Return()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            IDistanceService distanceService = new DistanceService(_httpClient);
            await distanceService.GetGeoPosition(FirstIATA);
        });
    }
    
    [Fact]
    public async Task GetDistance_WithoutIATAs_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            IDistanceService distanceService = new DistanceService(_httpClient);
            await distanceService.GetDistance(null, null);
        });
    }
    
    [Fact]
    public async Task GetDistance_WithoutOneIATA_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            IDistanceService distanceService = new DistanceService(_httpClient);
            await distanceService.GetDistance(FirstIATA, null);
        });
    }
    
    [Fact]
    public async Task GetDistance_WithSimilarIATA_ThrowException()
    {
        IDistanceService distanceService = new DistanceService(_httpClient);
        var result = await distanceService.GetDistance(FirstIATA, FirstIATA);
        
        Assert.Equal(0.0, result);
    }
    
    [Fact]
    public async Task GetDistance_WithDifferentIATAs_Return()
    {
        IDistanceService distanceService = new DistanceService(_httpClient);
        var excepted = 8675048.422827687;
        var actual = await distanceService.GetDistance(FirstIATA, FirstIATA);
        
        Assert.Equal(excepted, actual);
    }
}