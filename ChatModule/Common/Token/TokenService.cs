using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Common.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Common.Token;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(List<Claim> claims)
    {
        var token = new JwtTokenBuilder()
            .AddSecurityKey(JwtSecurityKey.Create(_configuration["Jwt:Secret"] ?? ""))
            .AddIssuer(_configuration["Jwt:ValidIssuer"] ?? "")
            .AddAudience(_configuration["Jwt:ValidAudience"] ?? "")
            .AddSubject(ClaimTypes.NameIdentifier)
            .AddClaims(claims)
            .AddExpiry(int.Parse(_configuration["Jwt:ExpirationInMinutes"] ?? ""))
            .Build();
        return token.Value;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? "")),
            ValidateLifetime = false,
            ValidIssuer = _configuration["Jwt:ValidIssuer"],
            ValidAudience = _configuration["Jwt:ValidAudience"],
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
}