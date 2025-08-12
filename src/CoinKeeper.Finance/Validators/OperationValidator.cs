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
            .GreaterThan(0).WithMessage("Сумма должна быть больше нуля");

        RuleFor(x => x.Description).NotEmpty()
            .MaximumLength(500).WithMessage("Описание не может быть пустым и должно быть не более 500 символов");

        RuleFor(x => x.OperationTime)
            .NotEmpty().WithMessage("Время операции должно быть указано");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Категория должна быть указана");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Счёт должен быть указан");

        RuleFor(x => x.OperationType)
            .IsInEnum().WithMessage("Указан некорректный тип операции");
    }
}
