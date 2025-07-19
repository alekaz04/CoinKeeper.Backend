using CoinKeeper.Hangfire.AspNetCore;
using CoinKeeper.Infrastructure;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
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
            .AddHangfireServer()
            .AddHostedService<HangfireBackgroundService>();

        return services;
    }

    public static IApplicationBuilder UseCoinKeeperHangfire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard();
        return app;
    }

    public static IServiceCollection AddHangfireJob<TJob>(this IServiceCollection services)
        where TJob : class, IHangfireRecurringJob
    {
        services.AddSingleton<IHangfireRecurringJob, TJob>();

        return services;
    }
}
