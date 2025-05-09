namespace CoinKeeper.Authentication.Domain;

public class UserPasswordDto
{
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;
}
