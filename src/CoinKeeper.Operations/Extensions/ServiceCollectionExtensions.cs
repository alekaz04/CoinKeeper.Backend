using CoinKeeper.Operations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CoinKeeper.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransactions(this IServiceCollection services)
    {
        services.AddScoped<OperationsCrudHandler>();
        services.AddValidatorsFromAssembly(typeof(OperationValidator).Assembly);
        services.AddAutoMapper(typeof(OperationMapper));
        return services;
    }
}
