using FluentValidation;

namespace CoinKeeper.Finance;

public class OperationValidator : AbstractValidator<OperationCreateDto>
{
    public OperationValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
