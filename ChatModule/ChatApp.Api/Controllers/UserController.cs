using ChatApp.Bus.Interfaces;
using Common.Base;
using Common.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Objects.Friends;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBus _user;
        public UserController(IUserBus user)
        {
            _user = user;
        }

        [HttpPost("FindUser")]
        [Authorize(Policy = Policy.MEMBER)]
        public async Task<BaseResult<FriendObj>> FindUser(BaseRequest<string> request)
        {
            var result = await _user.FindUser(request);
            return result;
        }
    }
}
