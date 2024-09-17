namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;

public interface IDistanceService
{
    Task<DistanceModel> GetDistanceAsync(IataModel model, CancellationToken cancellationToken = default);
}