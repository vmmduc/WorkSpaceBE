using ChatApp.Bus.Interfaces;
using ChatApp.Dal.Interfaces;
using Common.Base;
using Common.Token;
using Microsoft.Extensions.Configuration;
using Models.Objects;
using System.Security.Claims;

namespace ChatApp.Bus.Bussiness
{
    internal class AuthBus : IAuthBus
    {
        private readonly IAuthRepo _auth;
        private readonly ITokenService _token;

        private readonly IConfiguration _config;
        public AuthBus(IAuthRepo auth, ITokenService token, IConfiguration config)
        {
            _auth = auth;
            _token = token;
            _config = config;
        }

        public async Task<BaseResult<UserObj>> Login(BaseRequest<LoginObj> param)
        {
            try
            {
                var result = await _auth.Login(param);
                if (result.rtStatus)
                {
                    var claims = new List<Claim>();
                    if (result.rtResult?.lsRoles != null)
                        result.rtResult?.lsRoles.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x.RoleCode ?? "")));
                    claims.Add(new Claim(ClaimTypes.Name, result.rtResult?.fullName ?? ""));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, result.rtResult?.userId.ToString() ?? ""));
                    claims.Add(new Claim(ClaimTypes.Email, result.rtResult?.email ?? ""));

                    var token = _token.GenerateAccessToken(claims);
                    result.rtResult!.token = token;

                    var refreshToken = _token.GenerateRefreshToken();
                    await _auth.CreateRefreshToken(new RefreshTokenObj
                    {
                        UserId = result.rtResult.userId,
                        RefreshToken = refreshToken,
                        ExpiresTime = DateTime.Now.AddDays(int.Parse(_config["Jwt:RefreshTokenExpirationInDays"] ?? ""))
                    });
                    result.rtResult.refreshToken = refreshToken;
                }
                return result;
            }
            catch (Exception ex)
            {
                return new BaseResult<UserObj>(false, ex.Message);
            }
        }
    }
}
