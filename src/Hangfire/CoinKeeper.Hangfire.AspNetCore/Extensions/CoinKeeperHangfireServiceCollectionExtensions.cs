using CoinKeeper.Infrastructure;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinKeeper.Extensions.DependencyInjection;

public static class CoinKeeperHangfireServiceCollectionExtensions
{
    public static IServiceCollection AddCoinKeeperHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(x =>
            x.UsePostgreSqlStorage(options =>
                options.UseNpgsqlConnection(configuration.GetConnectionString(nameof(DataContext)))))
            .AddHangfireServer();

        return services;
    }

    public static IApplicationBuilder UseCoinKeeperHangfire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard(); //Will be available under http://localhost:5000/hangfire"
        return app;
    }
}
