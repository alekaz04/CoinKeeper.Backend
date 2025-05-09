namespace CoinKeeper.Authentication.Domain;
public class RequestUserDto
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
