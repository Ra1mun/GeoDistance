﻿namespace GeoDistance.Core.Tests.Unit;

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
        var memoryCache = Substitute.For<MemoryCache>();
        _geoCoordinateService = new GeoCoordinateService(httpClient, memoryCache);
    }

    private readonly IGeoCoordinateService _geoCoordinateService;

    private readonly string _correctAirportName = "AMS";

    private readonly string _badAirportName = "AQW";

    [Fact]
    public async Task GetGeoCoordinate_WithBadAirportName_ThrowException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
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