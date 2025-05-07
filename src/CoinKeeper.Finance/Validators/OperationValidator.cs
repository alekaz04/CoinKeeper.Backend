using FluentValidation;

namespace CoinKeeper.Finance;

public class OperationValidator : AbstractValidator<IOperation>
{
    public OperationValidator()
    {
    }
}
