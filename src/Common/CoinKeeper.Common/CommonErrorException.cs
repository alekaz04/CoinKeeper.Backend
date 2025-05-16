namespace CoinKeeper.Common;

/// <summary>
/// Базовая ошибка системы
/// </summary>
public class CommonErrorException : Exception
{
    public CommonErrorException(string message) : base(message)
    {

    }
}
