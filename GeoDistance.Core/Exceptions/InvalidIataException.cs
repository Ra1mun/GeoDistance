namespace GeoDistance.Core.Exceptions;

using System.Net;

public class InvalidIataException : ApplicationException
{
    public InvalidIataException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) :
        base(message)
    {
        StatusCode = httpStatusCode;
    }

    public HttpStatusCode StatusCode { get; }
}