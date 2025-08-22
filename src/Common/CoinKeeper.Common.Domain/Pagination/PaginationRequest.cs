namespace CoinKeeper.Common.Domain.Pagination;

public class PaginationRequest
{
    public int Skip { get; set; }
    public int Take { get; set; } = 50;
}
