namespace CoinKeeper.Finance;

public class BalanceUserDto
{
    public List<AccountBalanceDto> AccountBalances { get; set; } = [];
    public decimal Balance { get; set; }
}
