namespace CoinKeeper.Authentication.Domain;

public interface ICurrentUser
{
    public Guid GetCurrentUserId();
}
