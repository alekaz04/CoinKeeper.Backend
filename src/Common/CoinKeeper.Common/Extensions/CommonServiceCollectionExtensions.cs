using CoinKeeper.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace CoinKeeper.Extensions.DependencyInjection;

/// <summary>
/// Класс расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class CommonServiceCollectionExtensions
{
    /// <summary>
    /// Добавить основные сервисы
    /// </summary>
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSwaggerWithAuth()
            .AddScoped<CurrentUserResolver>();

        return services;
    }

    /// <summary>
    /// Добавить сваггер с аутентификацией
    /// </summary>
    private static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
    {
        services.AddOpenApiDocument(options =>
        {
            options.AddSecurity("Bearer", new OpenApiSecurityScheme
            {
                Description = "Bearer Auth token is needed",
                Type = OpenApiSecuritySchemeType.Http,
                In = OpenApiSecurityApiKeyLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer",
            });
            options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
        });

        return services;
    }
}
