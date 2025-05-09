using Microsoft.AspNetCore.Builder;


namespace CoinKeeper.Extensions.DependencyInjection;

public static class CommonApplicationBuilderExtensions
{
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
