using CoinKeeper.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Authentication;

public class UserController : CommonApiController
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<Guid> CreateUser([FromBody] RequestUserDto userDto, CancellationToken token)
    {
        return await _service.CreateUser(userDto, token);
    }
}
