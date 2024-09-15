using GeoDistance.Core.Dto;

namespace GeoDistance.Core.Services;

public interface IGeoCoordinateService
{
    Task<(GeoCoordinate, GeoCoordinate)> GetGeoCoordinates(IataModel model);
    Task<GeoCoordinate?> GetGeoCoordinate(string airport);
}