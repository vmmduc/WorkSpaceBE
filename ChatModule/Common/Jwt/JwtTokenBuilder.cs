using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Common.Jwt;

public class JwtTokenBuilder
{
    //https://github.com/TahirNaushad/Fiver.Security.Bearer

    private SecurityKey? _securityKey = null;
    private string _subject = "";
    private string _issuer = "";
    private string _audience = "";
    //private Dictionary<string, string> _claims = new Dictionary<string, string>();
    private List<Claim> _claims = new List<Claim>();
    private int _expiryInMinutes = 60;

    public JwtTokenBuilder AddSecurityKey(SecurityKey? securityKey)
    {
        this._securityKey = securityKey;
        return this;
    }

    public JwtTokenBuilder AddSubject(string subject)
    {
        this._subject = subject;
        return this;
    }

    public JwtTokenBuilder AddIssuer(string issuer)
    {
        this._issuer = issuer;
        return this;
    }

    public JwtTokenBuilder AddAudience(string audience)
    {
        this._audience = audience;
        return this;
    }

    public JwtTokenBuilder AddClaim(Claim claim)
    {
        this._claims.Add(claim);
        return this;
    }

    public JwtTokenBuilder AddClaims(List<Claim> claims)
    {
        this._claims.AddRange(claims);
        return this;
    }

    public JwtTokenBuilder AddExpiry(int expiryInMinutes)
    {
        this._expiryInMinutes = expiryInMinutes;
        return this;
    }

    public JwtToken Build()
    {
        EnsureArguments();

        this._claims.AddRange(new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, this._subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        });

        var token = new JwtSecurityToken(
                          issuer: this._issuer,
                          audience: this._audience,
                          claims: this._claims,
                          expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                          signingCredentials: new SigningCredentials(
                                                    this._securityKey,
                                                    SecurityAlgorithms.HmacSha256));
        return new JwtToken(token);
    }

    #region " private "

    private void EnsureArguments()
    {
        if (this._securityKey == null)
            throw new ArgumentNullException($"Security Key");

        if (string.IsNullOrEmpty(this._subject))
            throw new ArgumentNullException($"Subject");

        if (string.IsNullOrEmpty(this._issuer))
            throw new ArgumentNullException($"Issuer");

        if (string.IsNullOrEmpty(this._audience))
            throw new ArgumentNullException($"Audience");
    }

    #endregion
}