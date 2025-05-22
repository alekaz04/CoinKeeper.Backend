namespace CoinKeeper.Finance;

public class AccountUpdateDto
{
    public string Name { get; set; } = null!;
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
}
