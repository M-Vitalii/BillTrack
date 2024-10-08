using System.Net;
using BillTrack.Core.Exceptions;
using FastEndpoints; // Import your custom exceptions
using Microsoft.AspNetCore.Diagnostics;

namespace BillTrack.Api;

class ExceptionHandler;

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app,
                                                                 ILogger? logger = null,
                                                                 bool logStructuredException = true,
                                                                 bool useGenericReason = false)
    {
        app.UseExceptionHandler(
            errApp =>
            {
                errApp.Run(
                    async ctx =>
                    {
                        var exHandlerFeature = ctx.Features.Get<IExceptionHandlerFeature>();

                        if (exHandlerFeature is not null)
                        {
                            logger ??= ctx.Resolve<ILogger<ExceptionHandler>>();
                            var exception = exHandlerFeature.Error;
                            var route = exHandlerFeature.Endpoint?.DisplayName?.Split(" => ")[0];
                            var exceptionType = exception.GetType().Name;
                            var reason = exception.Message;

                            var (statusCode, errorMessage) = exception switch
                            {
                                NotFoundException notFoundException =>
                                    (StatusCodes.Status404NotFound, notFoundException.Message),

                                UnauthorizedAccessException unauthorizedException =>
                                    (StatusCodes.Status401Unauthorized, unauthorizedException.Message),

                                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
                            };

                            if (logStructuredException)
                            {
                                logger.LogError(exception, "[{@exceptionType}] at [{@route}] due to [{@reason}]", exceptionType, route, reason);
                            }
                            else
                            {
                                logger.LogError($"""
                                    =================================
                                    {route}
                                    TYPE: {exceptionType}
                                    REASON: {reason}
                                    ---------------------------------
                                    {exception.StackTrace}
                                    """);
                            }

                            ctx.Response.StatusCode = statusCode;
                            ctx.Response.ContentType = "application/problem+json";
                            await ctx.Response.WriteAsJsonAsync(
                                new InternalErrorResponse
                                {
                                    Status = "Error",
                                    Code = ctx.Response.StatusCode,
                                    Reason = useGenericReason ? "An unexpected error has occurred." : errorMessage,
                                    Note = "See application log for more details."
                                });
                        }
                    });
            });

        return app;
    }
}
