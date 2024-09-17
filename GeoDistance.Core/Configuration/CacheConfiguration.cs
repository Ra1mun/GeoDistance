namespace GeoDistance.Core.Configuration;

public class CacheConfiguration
{
    public const string KEY = "CacheConfiguration";

    public int AbsoluteExpirationInSeconds { get; init; } = 3600;

    public int SlidingExpirationInSeconds { get; init; } = 1800;
}