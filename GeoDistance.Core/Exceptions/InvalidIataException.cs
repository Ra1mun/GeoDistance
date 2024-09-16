using System.Net;

namespace GeoDistance.Core.Exceptions;

public class InvalidIataException : ApplicationException
{
    public InvalidIataException(string message, int statusCode = 500) : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}