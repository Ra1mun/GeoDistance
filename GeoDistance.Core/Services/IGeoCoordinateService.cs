namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;

public interface IGeoCoordinateService
{
    Task<GeoCoordinate?> GetGeoCoordinate(string airport);
}