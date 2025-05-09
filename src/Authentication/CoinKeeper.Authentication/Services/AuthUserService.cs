using AutoMapper;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Authentication;

public class AuthUserService
{
    private readonly IValidator<RequestUserDto> _validator;
    private readonly PasswordHashService _passwordHashService;
    private readonly DataContext _context;
    private readonly JsonWebTokenService _jwtService;

    public AuthUserService(IValidator<RequestUserDto> validator,
        PasswordHashService passwordHashService,
        DataContext context,
        JsonWebTokenService jwtService)
    {
        _validator = validator;
        _passwordHashService = passwordHashService;
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<UserResponseDto> Authenticate(RequestUserDto userDto, CancellationToken token)
    {
        _validator.ValidateAndThrow(userDto);

        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(x => x.Login == userDto.Login, token);

        if (user is null)
        {
            throw new CommonErrorException($"Пользователь с ником {userDto.Login} не найден");
        }

        string passwordUserHash = _passwordHashService.HashWithCurrentSalt(userDto.Password, user.PasswordSalt!);

        if (!passwordUserHash.Equals(user.PasswordHash, StringComparison.Ordinal))
        {
            throw new CommonErrorException("Неверный логин или пароль.");
        }

        var jwt = await _jwtService.GenerateToken(user.Id);

        user.RefreshToken = jwt.RefreshToken;
        await _context.SaveChangesAsync(token);

        var userResponseDto = new UserResponseDto()
        {
            Id = user.Id,
            Login = user.Login,
            Token = jwt
        };

        return userResponseDto;
    }

    public async Task<AuthToken> RefreshToken(string refreshToken, CancellationToken token)
    {
        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, token);

        if (user is null)
        {
            throw new CommonErrorException("Невалидный токен обновления");
        }

        var jwt = await _jwtService.GenerateToken(user.Id);

        user.RefreshToken = jwt.RefreshToken;

        await _context.SaveChangesAsync(token);

        return jwt;
    }
}
