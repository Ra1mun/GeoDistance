using System.Net;

namespace GeoDistance.Core.Exceptions;

public class InvalidIataException : ApplicationException
{
    public InvalidIataException(string message, HttpStatusCode statusCode = HttpStatusCode.Accepted) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}