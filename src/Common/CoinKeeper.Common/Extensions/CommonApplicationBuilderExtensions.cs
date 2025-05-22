using CoinKeeper.Common;
using Microsoft.AspNetCore.Builder;

namespace CoinKeeper.Extensions.DependencyInjection;

/// <summary>
/// Класс расширений для <see cref="IApplicationBuilder"/>
/// </summary>
public static class CommonApplicationBuilderExtensions
{
    /// <summary>
    /// Добавить сваггер
    /// </summary>
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
    {
        app.UseOpenApi();
        app.UseSwaggerUi(options =>
        {
            options.Path = string.Empty;
            options.DocumentTitle = "CoinKeeper API";
        });
        return app;
    }

    /// <summary>
    /// Добавить миддлвар отлова ошибок
    /// </summary>
    public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorMiddleware>();
        return app;
    }
}
