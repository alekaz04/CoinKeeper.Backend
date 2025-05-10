namespace CoinKeeper.Finance;

public class CategoryReadDto : ICategory
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
}
