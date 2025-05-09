using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace CoinKeeper.Authentication;

public class JsonWebTokenService
{
    private readonly JwtOptions _jwtOptions;

    public JsonWebTokenService(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public async Task<AuthToken> GenerateToken(Guid userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var token = new JwtSecurityToken(_jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.UtcNow.Add(_jwtOptions.Expiration),
            signingCredentials: credentials);

        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        string refreshToken = GenerateRefreshToken();

        return new AuthToken()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = _jwtOptions.Expiration
        };
    }

    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[64];

        RandomNumberGenerator.Fill(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}
