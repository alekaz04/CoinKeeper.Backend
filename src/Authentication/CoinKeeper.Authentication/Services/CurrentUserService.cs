using CoinKeeper.Authentication.Domain;
using CoinKeeper.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CoinKeeper.Authentication;

/// <inheritdoc />
public class CurrentUserService : ICurrentUser
{
    /// <inheritdoc cref="IHttpContextAccessor"/>
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public Guid GetCurrentUserId()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new CommonErrorException("Не авторизованный пользователь");
        }

        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
        {
            throw new CommonErrorException("Claim User is not found");
        }

        return new Guid(userIdClaim.Value);
    }
}
