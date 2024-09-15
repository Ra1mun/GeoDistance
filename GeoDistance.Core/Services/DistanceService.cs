﻿namespace GeoDistance.Core.Services;

using System.Net.Http.Json;

using GeoDistance.Core.Dto;
using GeoDistance.Core.Exceptions;

public class DistanceService : IDistanceService
{
    private readonly IGeoCoordinateService _geoCoordinateService;

    public DistanceService(IGeoCoordinateService geoCoordinateService)
    {
        _geoCoordinateService = geoCoordinateService;
    }

    public async Task<DistanceModel> GetDistance(IataModel firstIata, IataModel secondIata)
    {
        var firstPoint = await _geoCoordinateService.GetGeoCoordinate(firstIata);
        var secondPoint = await _geoCoordinateService.GetGeoCoordinate(secondIata);

        return CalculateDistance(firstPoint.Location, secondPoint.Location);
    }

    private static DistanceModel CalculateDistance(Location point1, Location point2)
    {
        var d1 = point1.Latitude * (Math.PI / 180.0);
        var num1 = point1.Longitude * (Math.PI / 180.0);
        var d2 = point2.Latitude * (Math.PI / 180.0);
        var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                 Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        var distance = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        return new DistanceModel
        {
            Value = distance,
        };
    }
}