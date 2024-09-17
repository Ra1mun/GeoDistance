namespace GeoDistance.Core.Services;

using GeoDistance.Core.Dto;

public class DistanceService : IDistanceService
{
    private static readonly DistanceModel DefaultDistance = new()
    {
        Value = 0,
    };

    private readonly IGeoCoordinateService _geoCoordinateService;

    public DistanceService(IGeoCoordinateService geoCoordinateService)
    {
        _geoCoordinateService = geoCoordinateService;
    }

    public async Task<DistanceModel> GetDistanceAsync(IataModel model, CancellationToken cancellationToken = default)
    {
        if (string.Equals(model.FirstAirport, model.SecondAirport, StringComparison.OrdinalIgnoreCase))
            return DefaultDistance;

        var coordinate1 = await _geoCoordinateService.GetGeoCoordinateAsync(model.FirstAirport, cancellationToken);
        var coordinate2 = await _geoCoordinateService.GetGeoCoordinateAsync(model.SecondAirport, cancellationToken);

        return CalculateDistance(coordinate1.Location, coordinate2.Location);
    }

    private static DistanceModel CalculateDistance(Location point1, Location point2)
    {
        const double angle = Math.PI / 180.0;
        const double earthLength = 6376500.0;
        var d1 = point1.Latitude * angle;
        var num1 = point1.Longitude * angle;
        var d2 = point2.Latitude * angle;
        var num2 = point2.Longitude * angle - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                 Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        var distance = earthLength * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        return new DistanceModel
        {
            Value = distance,
        };
    }
}