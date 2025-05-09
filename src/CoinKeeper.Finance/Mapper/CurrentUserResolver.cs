using AutoMapper;
using CoinKeeper.Authentication.Domain;

namespace CoinKeeper.Finance;

public class CurrentUserResolver : IValueResolver<object, object, Guid>
{
    private readonly ICurrentUser _currentUser;

    public CurrentUserResolver(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public Guid Resolve(object source, object destination, Guid destMember, ResolutionContext context)
    {
        return _currentUser.GetCurrentUserId();
    }
}
