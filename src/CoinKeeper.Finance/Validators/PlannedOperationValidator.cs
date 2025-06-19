using FluentValidation;

namespace CoinKeeper.Finance;

public class PlannedOperationValidator : AbstractValidator<PlannedOperationCreateDto>
{
    public PlannedOperationValidator()
    {

    }
}
