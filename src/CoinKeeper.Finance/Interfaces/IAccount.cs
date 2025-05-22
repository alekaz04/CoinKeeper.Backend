namespace CoinKeeper.Finance;

public interface IAccount
{
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
}
