using CoinKeeper.Authentication.Domain;
using System.Security.Cryptography;

namespace CoinKeeper.Authentication;

/// <summary>
/// Сервис для хэширования пароля
/// </summary>
public static class PasswordHashService
{
    /// <summary>
    /// Размер соли
    /// </summary>
    private const int SaltSize = 16;

    /// <summary>
    /// Размер ключа
    /// </summary>
    private const int KeySize = 32;

    /// <summary>
    /// Количество итераций
    /// </summary>
    private const int Iterations = 100001;

    /// <summary>
    /// Захэшировать пароль
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <returns>Хэш пароля + соль</returns>
    public static UserPasswordDto Hash(string password)
    {
        byte[] salt = GenerateSalt();
        byte[] hash = new Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256
        ).GetBytes(KeySize);

        return new UserPasswordDto()
        {
            Password = Convert.ToBase64String(hash),
            Salt = Convert.ToBase64String(salt)
        };
    }

    /// <summary>
    /// Хэшировать пароль с определенной солью
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <param name="salt">Соль</param>
    public static string HashWithCurrentSalt(string password, string salt)
    {
        byte[] hash = new Rfc2898DeriveBytes(
            password,
            Convert.FromBase64String(salt),
            Iterations,
            HashAlgorithmName.SHA256
        ).GetBytes(KeySize);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Получить случайную соль
    /// </summary>
    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltSize];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }
}
