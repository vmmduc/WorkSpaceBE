using Microsoft.AspNetCore.Http;

namespace Common.Token
{
    public class TokenManager : IMiddleware
    {
        public TokenManager() { }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
            return;
        }
    }
}
