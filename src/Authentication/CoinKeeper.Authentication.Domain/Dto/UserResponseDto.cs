namespace CoinKeeper.Authentication;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Login { get; set; } = null!;
    public AuthToken? Token { get; set; }
}
