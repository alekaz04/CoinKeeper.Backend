using CoinKeeper.Extensions.DependencyInjection;
using CoinKeeper.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Backend.Api;

/// <summary>
/// Конфигурация сервера
/// </summary>
public class Startup
{
    /// <inheritdoc cref="IConfiguration"/>
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCommon(Configuration)
            .AddFinance()
            .AddAuth(Configuration)
            .AddCoinKeeperLogging(Configuration)
            .AddCoinKeeperHangfire(Configuration);

        services.AddDbContextFactory<DataContext>(x => x.UseNpgsql(Configuration.GetConnectionString(nameof(DataContext))));

        services.AddDbContext<DataContext>(x => x.UseNpgsql(Configuration.GetConnectionString(nameof(DataContext))));
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();

        app.UseErrorMiddleware();

        app.UseCoinKeeperHangfire();

        app.UseRouting();
        app.UseAuthentication().UseAuthorization();
        app.UseEndpoints(x => x.MapControllers());
    }
}
