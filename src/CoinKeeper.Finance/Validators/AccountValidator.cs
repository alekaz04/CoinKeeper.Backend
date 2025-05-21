using FluentValidation;

namespace CoinKeeper.Finance;

public class AccountValidator : AbstractValidator<AccountCreateDto>
{
    public AccountValidator()
    {
        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("Баланс может быть больше либо равно 0");
    }
}
