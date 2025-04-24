using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;

namespace CoinKeeper.Common;

public class ErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (CommonErrorException e)
        {
            _logger.LogError(e, e.Message);
            await HandleException(context, e.Message, 400);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleException(context, "Internal Server Error", 500);
        }
    }

    private static Task HandleException(HttpContext context, string message, int statusCode)
    {
        var errorResponse = new ErrorResponse
        {
            Message = message,
            TraceId = Activity.Current?.TraceId.ToString()
        };

        string json = JsonSerializer.Serialize(errorResponse);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(json);
    }

}
