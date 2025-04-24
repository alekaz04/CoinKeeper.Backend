namespace CoinKeeper.Common;

public class ErrorResponse
{
    public string Message { get; set; } = null!;
    public string? TraceId { get; set; }
}
