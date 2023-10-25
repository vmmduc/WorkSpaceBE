using System.Security.Claims;

namespace Common.Token;

public interface ITokenService
{
    string GenerateAccessToken(List<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}