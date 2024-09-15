namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;

public interface IDistanceService
{
    Task<DistanceModel> GetDistance(IataModel firstIata, IataModel secondIata);
}