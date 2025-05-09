namespace CoinKeeper.Authentication.Domain;

public interface IUserSpecifiedEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
