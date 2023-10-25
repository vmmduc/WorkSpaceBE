using System.Security.Claims;
using Common.Token;
using Microsoft.AspNetCore.Http;

namespace Common.HttpContextAccessor;

public class CurrentUserService : ICurrentUserService
{
    public bool IsAuthenticated { get; }
    public int UserId { get; }
    public string? Email { get; }
    public string? FullName { get; }
    public List<string> Roles { get; set; } = new List<string>();

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
    {
        var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(token);
            {
                var value = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (value != null)
                {
                    UserId = int.Parse(value);
                    Email = principal.FindFirst(ClaimTypes.Email)?.Value;
                    FullName = principal.FindFirst(ClaimTypes.Name)?.Value;
                    var roles = principal.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
                    Roles = new List<string>();
                    Roles.AddRange(roles.Select(x => x.Value).ToList());
                }

                if (principal.Identity != null) IsAuthenticated = principal.Identity.IsAuthenticated;
            }
        }
    }
}