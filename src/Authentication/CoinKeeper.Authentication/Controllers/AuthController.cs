using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Authentication;

/// <summary>
/// Авторизация
/// </summary>
public class AuthController : CommonApiController
{
    /// <inheritdoc cref="AuthUserService"/>
    private readonly AuthUserService _service;

    public AuthController(AuthUserService service)
    {
        _service = service;
    }

    /// <summary>
    /// Войти в систему
    /// </summary>
    /// <param name="userDto">Логин и пароль</param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Пользователь + Пара Access и Refresh токен</returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<UserResponseDto> Authenticate([FromBody] RequestUserDto userDto, CancellationToken token)
    {
        return await _service.Authenticate(userDto, token);
    }

    /// <summary>
    /// Получить новый Access токен по refresh токену
    /// </summary>
    /// <param name="refreshToken">Токен обновления</param>
    /// <param name="token">Токен отмены запросы</param>
    /// <returns>Новая пара Access и Refresh токена</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<AuthToken> RefreshToken([FromQuery] string refreshToken, CancellationToken token)
    {
        return await _service.RefreshToken(refreshToken, token);
    }
}
