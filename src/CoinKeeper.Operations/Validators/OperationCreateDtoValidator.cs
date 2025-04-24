using CoinKeeper.Operations.Dto;
using FluentValidation;

namespace CoinKeeper.Operations.Validators;

public class OperationCreateDtoValidator : AbstractValidator<OperationCreateDto>
{
    public OperationCreateDtoValidator()
    {
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0)
            .WithMessage("Amount must be greater than 0");
    }
}
