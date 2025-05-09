using CoinKeeper.Finance;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CoinKeeper.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFinance(this IServiceCollection services)
    {
        services.AddScoped<OperationsCrudHandler>();
        services.AddScoped<CategoryCrudHandler>();
        services.AddValidatorsFromAssembly(typeof(OperationValidator).Assembly);
        services.AddAutoMapper(typeof(OperationMapper), typeof(CategoryMapper));
        services.AddScoped<CurrentUserResolver>();
        return services;
    }
}
