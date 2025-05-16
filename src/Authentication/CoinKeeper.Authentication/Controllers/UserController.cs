using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Authentication;

/// <summary>
/// Пользователи
/// </summary>
public class UserController : CommonApiController
{
    /// <inheritdoc cref="UserService"/>
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="userDto">Запрос на создания пользователя</param>
    /// <param name="token">Токен отмены запроса</param>
    /// <returns>Идентфикатор созданного пользователя</returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<Guid> CreateUser([FromBody] RequestUserDto userDto, CancellationToken token)
    {
        return await _service.CreateUser(userDto, token);
    }
}
