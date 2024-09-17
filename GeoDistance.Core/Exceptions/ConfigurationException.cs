namespace GeoDistance.Core.Exceptions;

public class ConfigurationException : BaseGeoDistanceException
{
    public ConfigurationException(string message, int statusCode = 500) : base(message, statusCode)
    {
    }
}

