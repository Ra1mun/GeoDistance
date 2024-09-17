namespace GeoDistance.Core.Tests.Integration;

using System.Net;
using System.Net.Http.Json;

using Flurl;

using GeoDistance.Api;
using GeoDistance.Core.Dto;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

public class GeoDistanceControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    public GeoDistanceControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private const string AIRPORT_NAME = "ams";
    private readonly HttpClient _client;

    [Theory]
    [InlineData("", AIRPORT_NAME)]
    [InlineData(AIRPORT_NAME, "")]
    [InlineData(AIRPORT_NAME, null)]
    [InlineData(null, AIRPORT_NAME)]
    [InlineData(null, null)]
    [InlineData("", "")]
    public async Task GetDistance_ReturnsBadRequest(string firstAirport, string secondAirport)
    {
        // arrange
        var url = CreateUrl(firstAirport, secondAirport);

        // act
        var response = await _client.GetAsync(url);
        var statusCode = response.StatusCode;

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, statusCode);
    }

    private static Url CreateUrl(string firstAirport, string secondAirport)
    {
        const string controllerPath = "/GeoDistance/v1";

        return new Url(controllerPath)
        {
            QueryParams =
            {
                { "FirstAirport", firstAirport },
                { "SecondAirport", secondAirport },
            },
        };
    }

    [Fact]
    public async Task GetDistance_ReturnsOk()
    {
        // arrange
        const double expected = 923815.94;
        var url = CreateUrl(AIRPORT_NAME, "BOD");

        // act
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var actual = await response.Content.ReadFromJsonAsync<DistanceModel>();

        // assert
        Assert.Equal(expected, actual.Value, 2);
    }
}