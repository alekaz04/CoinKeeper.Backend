using FluentValidation;

namespace CoinKeeper.Authentication.Validators;

public class RequestUserDtoValidator : AbstractValidator<RequestUserDto>
{
    public RequestUserDtoValidator()
    {
        RuleFor(x => x.Login)
            .NotNull()
            .NotEmpty()
            .Matches("^[a-zA-Z0-9_]+$");

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6)
            .Matches("^[a-zA-Z0-9!@#$%^&*-_=+]+$");
    }
}
