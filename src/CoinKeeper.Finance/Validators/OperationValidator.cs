using FluentValidation;

namespace CoinKeeper.Finance;

/// <summary>
/// Валидатор создания операции
/// </summary>
public class OperationValidator : AbstractValidator<OperationCreateDto>
{
    public OperationValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
