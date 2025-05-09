using System.Security.Cryptography;

namespace CoinKeeper.Authentication;

public class PasswordHashService
{
    private const int SaltSize = 16;

    private const int KeySize = 32;

    private const int Iterations = 500;

    public UserPasswordDto Hash(string password)
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
            Salt =  Convert.ToBase64String(salt)
        };
    }

    public string HashWithCurrentSalt(string password, string salt)
    {
        byte[] hash = new Rfc2898DeriveBytes(
            password,
            Convert.FromBase64String(salt),
            Iterations,
            HashAlgorithmName.SHA256
        ).GetBytes(KeySize);
        return Convert.ToBase64String(hash);
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltSize];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }
}
