using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
namespace CoinKeeper.Extensions.DependencyInjection;

public static class CommonServiceCollectionExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(configuration)
            .AddSwaggerWithAuth();
        return services;
    }

    private static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var log = Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .CreateLogger();

        services.AddSerilog(log);

        return services;
    }

    private static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
    {
        services.AddOpenApiDocument(options =>
        {
            options.AddSecurity("Bearer", new OpenApiSecurityScheme
            {
                Description = "",
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
