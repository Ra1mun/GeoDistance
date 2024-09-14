namespace GeopositionDistace;

public interface IDistanceService
{
    Task<double> GetDistance(string firstIATA, string secondIATA);
    Task<GeoPosition?> GetGeoPosition(string iata);
}