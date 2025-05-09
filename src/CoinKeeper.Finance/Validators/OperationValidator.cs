using FluentValidation;

namespace CoinKeeper.Finance;

public class OperationValidator : AbstractValidator<OperationCreateDto>
{
    public OperationValidator()
    {
    }
}
