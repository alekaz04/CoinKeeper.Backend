using CoinKeeper.Common;
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
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCommon(Configuration)
            .AddFinance()
            .AddAuth(Configuration);

        services.AddDbContext<DataContext>(x => x.UseNpgsql(Configuration.GetConnectionString(nameof(DataContext))));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();

        app.UseMiddleware<ErrorMiddleware>();

        app.UseRouting();
        app.UseAuthentication().UseAuthorization();
        app.UseEndpoints(x => x.MapControllers());
    }
}
