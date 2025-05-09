using FluentValidation;

namespace CoinKeeper.Finance;

public class CategoryValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("CategoryName cannot be empty");
    }
}
