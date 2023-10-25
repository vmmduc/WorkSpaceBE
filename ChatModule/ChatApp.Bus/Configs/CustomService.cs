using ChatApp.Bus.Bussiness;
using ChatApp.Bus.Interfaces;
using ChatApp.Bus.SignalRHub;
using ChatApp.Dal.Interfaces;
using ChatApp.Dal.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Common.Constant;
using Common.Token;
using Common.HttpContextAccessor;

namespace ChatApp.Bus.Configs
{
    public static class CustomService
    {
        public static void AddCustomService(this IServiceCollection service)
        {
            service.AddScoped<IAuthBus, AuthBus>();
            service.AddScoped<IUserBus, UserBus>();
            service.AddScoped<IFriendBus, FriendBus>();

            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<ICurrentUserService, CurrentUserService>();
            service.AddScoped<IConversationBus, ConversationBus>();

            service.AddScoped<IAuthRepo, AuthRepo>();
            service.AddScoped<IUserRepo, UserRepo>();
            service.AddScoped<IFriendRepo, FriendRepo>();
            service.AddScoped<IConversationRepo, ConversationRepo>();
        }
        public static void AddPolicy(this IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Policy.MEMBER, policy => policy.RequireAuthenticatedUser());
            });
        }
    }
}
