using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using CoinKeeper.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CoinKeeper.Authentication;

/// <summary>
/// Сервис аутентификации пользователя
/// </summary>
public class AuthUserService
{
    /// <inheritdoc cref="PasswordHashService"/>
    private readonly PasswordHashService _passwordHashService;

    /// <inheritdoc cref="DataContext"/>
    private readonly DataContext _context;

    /// <inheritdoc cref="JsonWebTokenService"/>
    private readonly JsonWebTokenService _jwtService;

    public AuthUserService(PasswordHashService passwordHashService,
        DataContext context,
        JsonWebTokenService jwtService)
    {
        _passwordHashService = passwordHashService;
        _context = context;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Аутентифицировать пользователя по логину и паролю
    /// </summary>
    /// <param name="userDto">Логин и пароль пользователя</param>
    /// <param name="token"></param>
    /// <returns>Информацию о пользователя + Пара Access и Refresh токен</returns>
    public async Task<UserResponseDto> Authenticate(RequestUserDto userDto, CancellationToken token)
    {
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

    /// <summary>
    /// Создать новый Access токен для пользователя по Refresh токену
    /// </summary>
    /// <param name="refreshToken">Refresh токен</param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns></returns>
    /// <exception cref="CommonErrorException"></exception>
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
