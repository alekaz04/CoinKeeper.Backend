using CoinKeeper.Operations.Handlers;
using CoinKeeper.Operations.Mapper;
using CoinKeeper.Operations.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CoinKeeper.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransactions(this IServiceCollection services)
    {
        services.AddScoped<OperationsCrudHandler>();
        services.AddValidatorsFromAssembly(typeof(OperationCreateDtoValidator).Assembly);
        services.AddAutoMapper(typeof(OperationMapper));
        return services;
    }
}
