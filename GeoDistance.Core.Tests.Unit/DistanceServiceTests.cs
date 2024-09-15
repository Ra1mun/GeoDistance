namespace GeoDistance.Core.Tests.Unit;

using GeoDistance.Core.Services;

using NSubstitute;

using Xunit;

public class DistanceServiceTests
{
    public DistanceServiceTests()
    {
        var httpClient = Substitute.For<HttpClient>();
        _distanceService = new DistanceService(httpClient);
    }

    private readonly IDistanceService _distanceService;
    private readonly string FirstIATA = "AMS";
    private readonly string SecondIATA = "MSC";

    [Fact]
    public async Task GetDistance_WithDifferentIATAs_Return()
    {
        var excepted = 8675048.422827687;
        var actual = await _distanceService.GetDistance(FirstIATA, FirstIATA);

        Assert.Equal(excepted, actual);
    }

    [Fact]
    public async Task GetDistance_WithoutIATAs_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _distanceService.GetDistance(null, null);
        });
    }

    [Fact]
    public async Task GetDistance_WithoutOneIATA_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _distanceService.GetDistance(FirstIATA, null);
        });
    }

    [Fact]
    public async Task GetDistance_WithSimilarIATA_ThrowException()
    {
        var result = await _distanceService.GetDistance(FirstIATA, FirstIATA);

        Assert.Equal(0.0, result);
    }
}