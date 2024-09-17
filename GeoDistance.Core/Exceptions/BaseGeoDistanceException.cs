namespace GeoDistance.Core.Exceptions;

public abstract class BaseGeoDistanceException : ApplicationException
{
    protected BaseGeoDistanceException(string message, int statusCode = 500) : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}