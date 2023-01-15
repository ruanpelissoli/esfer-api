using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Esfer.API.Middlewares;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Internal Server Error",
                Title = "Error",
                Detail = e.Message
            };

            var json = JsonSerializer.Serialize(problem);

            await context.Response.WriteAsync(json);

            context.Response.ContentType = "application/json";
        }
    }
}
