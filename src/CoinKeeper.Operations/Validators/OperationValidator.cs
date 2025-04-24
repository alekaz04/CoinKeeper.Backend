using FluentValidation;

namespace CoinKeeper.Operations;

public class OperationValidator : AbstractValidator<IOperation>
{
    public OperationValidator()
    {
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0)
            .WithMessage("Amount must be greater than 0");
    }
}
