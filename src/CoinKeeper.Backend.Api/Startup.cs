using CoinKeeper.Extensions.DependencyInjection;
using CoinKeeper.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Backend.Api;

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
            .AddTransactions();

        services.AddDbContext<DataContext>(x => x.UseNpgsql(Configuration.GetConnectionString(nameof(DataContext))));

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        services.AddSwaggerDocument(x =>
        {
            x.Title = "Coin Keeper API";
            x.Version = "0.0.0";
            x.Description = "Coin Keeper API";
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseOpenApi(x =>
        {
            x.Path = "openapi/v1.json";
        });
        app.UseSwaggerUi(options =>
        {
            options.DocumentPath = "openapi/v1.json";
        });

        app.UseRouting();
        app.UseAuthentication().UseAuthorization();
        app.UseEndpoints(x => x.MapControllers());
    }
}
