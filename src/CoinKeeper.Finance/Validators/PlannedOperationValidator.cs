using FluentValidation;

namespace CoinKeeper.Finance;

public class PlannedOperationValidator : AbstractValidator<PlannedOperationCreateDto>
{
    public PlannedOperationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название планового платежа обязательно для заполнения")
            .Length(1, 200)
            .WithMessage("Название должно содержать от 1 до 200 символов");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Сумма должна быть больше нуля");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Категория обязательна для выбора");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Счет обязателен для выбора");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow.Date)
            .WithMessage("Дата начала не может быть в прошлом");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("Дата окончания должна быть больше даты начала");

        RuleFor(x => x.NextExecutionDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("Дата следующего выполнения должна быть не раньше даты начала");

        RuleFor(x => x.Frequency)
            .GreaterThan(0)
            .WithMessage("Частота должна быть больше нуля");

        RuleFor(x => x.MaxExecutions)
            .GreaterThan(0)
            .When(x => x.MaxExecutions.HasValue)
            .WithMessage("Максимальное количество выполнений должно быть больше нуля");

        RuleFor(x => x.ScheduledTime)
            .Must(time => time >= TimeSpan.Zero && time < TimeSpan.FromDays(1))
            .WithMessage("Время выполнения должно быть в пределах суток (00:00:00 - 23:59:59)");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Описание не должно превышать 500 символов");
    }
}
