namespace GeoDistance.Api.Middleware;

using System.Text.Json;

using GeoDistance.Core.Exceptions;

using Microsoft.AspNetCore.Mvc;

internal class LoggingMiddleware
{
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await WriteExceptionToResponse(context, e);
        }
    }

    private static async Task WriteExceptionToResponse(HttpContext context, Exception e)
    {
        var statusCode = 500;
        var details = new ProblemDetails
        {
            Title = "Возникла непредвиденная ошибка",
            Detail = "Подробности см. в логах",
            Status = statusCode,
        };

        if (e is InvalidIataException iataException)
        {
            details.Title = "Ошибка";
            details.Detail = e.Message;
            details.Status = iataException.StatusCode;

            statusCode = iataException.StatusCode;
        }

        var json = JsonSerializer.Serialize(details);

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}