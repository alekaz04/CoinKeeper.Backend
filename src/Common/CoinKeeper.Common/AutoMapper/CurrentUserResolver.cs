using AutoMapper;
using CoinKeeper.Authentication.Domain;

namespace CoinKeeper.Common;

/// <summary>
/// Ресолвер для автомаппера по получению идентификатора конкретного пользователя
/// </summary>
public class CurrentUserResolver : IValueResolver<object, object, Guid>
{
    /// <inheritdoc cref="ICurrentUser"/>
    private readonly ICurrentUser _currentUser;

    public CurrentUserResolver(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    /// <inheritdoc />
    public Guid Resolve(object source, object destination, Guid destMember, ResolutionContext context)
    {
        return _currentUser.GetCurrentUserId();
    }
}
