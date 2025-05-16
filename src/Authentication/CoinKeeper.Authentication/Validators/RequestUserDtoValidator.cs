using CoinKeeper.Authentication.Domain;
using FluentValidation;

namespace CoinKeeper.Authentication.Validators;

/// <summary>
/// Валидатор логина и пароля пользователя
/// </summary>
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
