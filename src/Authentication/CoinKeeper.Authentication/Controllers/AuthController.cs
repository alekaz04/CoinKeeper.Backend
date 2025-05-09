using CoinKeeper.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinKeeper.Authentication;

public class AuthController : CommonApiController
{
    private readonly AuthUserService _service;

    public AuthController(AuthUserService service)
    {
        _service = service;
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<UserResponseDto> Authenticate([FromBody] RequestUserDto userDto, CancellationToken token)
    {
        return await _service.Authenticate(userDto, token);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<AuthToken> RefreshToken([FromQuery] string refreshToken, CancellationToken token)
    {
        return await _service.RefreshToken(refreshToken, token);
    }
}
