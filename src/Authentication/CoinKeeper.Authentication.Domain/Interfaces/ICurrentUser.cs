namespace CoinKeeper.Authentication.Domain;

public interface ICurrentUser
{
    public Task<Guid> GetCurrentUserId();
}
