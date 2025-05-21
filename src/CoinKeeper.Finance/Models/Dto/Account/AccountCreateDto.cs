namespace CoinKeeper.Finance;

public class AccountCreateDto :  IAccount
{
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
}
