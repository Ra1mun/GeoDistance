namespace GeoDistance.Core.Exceptions;

public class InvalidIataException : BaseGeoDistanceException
{
    public InvalidIataException(string message, int statusCode = 500) : base(message, statusCode)
    {
    }
}
