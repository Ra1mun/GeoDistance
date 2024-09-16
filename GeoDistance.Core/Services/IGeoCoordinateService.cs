using GeoDistance.Core.Dto;

namespace GeoDistance.Core.Services;

public interface IGeoCoordinateService
{
    Task<GeoCoordinate?> GetGeoCoordinate(string airport);
}