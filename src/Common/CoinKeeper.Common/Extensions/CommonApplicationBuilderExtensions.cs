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
        });
        return app;
    }
}
