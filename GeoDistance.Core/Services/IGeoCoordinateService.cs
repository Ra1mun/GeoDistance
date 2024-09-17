namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;

public interface IGeoCoordinateService
{
    Task<GeoCoordinate> GetGeoCoordinateAsync(string airport, CancellationToken cancellationToken = default);
}