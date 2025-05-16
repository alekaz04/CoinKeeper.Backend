using CoinKeeper.Common;
using CoinKeeper.Finance;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CoinKeeper.Extensions.DependencyInjection;

/// <summary>
/// Класс расширений для <see cref="IServiceProvider"/> для модуля финансов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить модуль финансов
    /// </summary>
    public static IServiceCollection AddFinance(this IServiceCollection services)
    {
        services.AddScoped<AbstractCrudHandler<Operation, OperationReadDto, OperationCreateDto>, OperationsCrudHandler>();
        services.AddScoped<AbstractCrudHandler<Category, CategoryReadDto, CategoryCreateDto>, CategoryCrudHandler>();

        services.AddValidatorsFromAssembly(typeof(OperationValidator).Assembly);

        services.AddAutoMapper(typeof(OperationMapper), typeof(CategoryMapper));

        services.AddScoped<CurrentUserResolver>();
        return services;
    }
}
