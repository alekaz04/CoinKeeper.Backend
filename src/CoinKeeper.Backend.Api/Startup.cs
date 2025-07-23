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

    /// <summary>
    /// Инициализация приложения
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Инициализация сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCommon(Configuration)
            .AddFinance()
            .AddAuth(Configuration)
            .AddCoinKeeperLogging(Configuration)
            .AddCoinKeeperHangfire(Configuration);

        services.AddDbContext<DataContext>(x => x.UseNpgsql(Configuration.GetConnectionString(nameof(DataContext))));
    }

    /// <summary>
    /// Конфигурация сервисов
    /// </summary>
    /// <param name="app"><inheritdoc cref="IApplicationBuilder"/></param>
    /// <param name="env"><inheritdoc cref="IWebHostEnvironment"/></param>
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
