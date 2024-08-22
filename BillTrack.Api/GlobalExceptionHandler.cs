using System.Net.Mime;
using BillTrack.Core.Exceptions;
using FastEndpoints;
using Microsoft.AspNetCore.Diagnostics;

namespace BillTrack.Api;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, errorMessage) = exception switch
        {
            NotFoundException notFoundException => (StatusCodes.Status404NotFound, notFoundException.Message),
            _ => (StatusCodes.Status500InternalServerError, "Something went wrong")
        };

        _logger.LogError(exception, "An error occurred: {Message}", errorMessage);

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = statusCode;

        ErrorResponse response = new()
        {
            StatusCode = statusCode,
            Message = errorMessage
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}