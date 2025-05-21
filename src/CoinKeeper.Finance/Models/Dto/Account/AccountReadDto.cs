namespace CoinKeeper.Finance;

public class AccountReadDto : IAccount
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
}
