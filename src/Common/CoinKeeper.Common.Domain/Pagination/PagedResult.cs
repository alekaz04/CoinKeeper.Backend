namespace CoinKeeper.Common.Domain;

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public List<T> Items { get; set; } = [];
}
