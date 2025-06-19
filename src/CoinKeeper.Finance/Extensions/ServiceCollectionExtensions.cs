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
        services.AddScoped<AbstractCrudHandler<Operation, OperationReadDto, OperationCreateDto, OperationUpdateDto>, OperationsCrudHandler>();
        services.AddScoped<AbstractCrudHandler<Category, CategoryReadDto, CategoryCreateDto, CategoryUpdateDto>, CategoryCrudHandler>();
        services.AddScoped<AbstractCrudHandler<Account, AccountReadDto, AccountCreateDto, AccountUpdateDto>, AccountCrudHandler>();
        services.AddScoped<AbstractCrudHandler<PlannedOperation, PlannedOperationReadDto, PlannedOperationCreateDto, PlannedOperationUpdateDto>, PlannedOperationCrudHandler>();

        services.AddValidatorsFromAssembly(typeof(ICoinKeeperFinanceModuleAssemblyMark).Assembly);

        services.AddAutoMapper(typeof(OperationMapper), typeof(CategoryMapper));
        services.AddScoped<IAccountBalanceService, AccountBalanceService>();
        services.AddScoped<IBalanceHandler, BalanceHandler>();

        services.AddHostedService<RecalculationAllUsersBalanceHostedService>();
        return services;
    }
}
