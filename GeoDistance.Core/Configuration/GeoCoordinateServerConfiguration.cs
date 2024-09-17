namespace GeoDistance.Core.Configuration;

public class GeoCoordinateServerConfiguration
{
    public const string KEY = "GeoCoordinateServerConfiguration";
    
    public string Url { get; init; } = string.Empty;
}