using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;

namespace CoinKeeper.Common;

/// <summary>
/// Миддлвар для отлова ошибок
/// </summary>
public class ErrorMiddleware : IMiddleware
{
    /// <inheritdoc cref="ILogger{T}"/>
    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(ILogger<ErrorMiddleware> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (CommonErrorException e)
        {
            _logger.LogError(e, e.Message);
            await HandleException(context, e.Message, 400);
        }
        catch (ValidationException e)
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

    /// <summary>
    /// Запокавать ошибку в ответ
    /// </summary>
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
