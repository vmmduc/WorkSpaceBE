using ChatApp.Bus.Interfaces;
using Common.Base;
using Common.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Objects;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBus _auth;
        public AuthController(IAuthBus auth)
        {
            _auth = auth;
        }
        [HttpPost("Login")]
        public async Task<BaseResult<UserObj>> Login(BaseRequest<LoginObj> param)
        {
            var result = await _auth.Login(param);
            return result;
        }

        [HttpGet("Ping")]
        [Authorize(Policy = Policy.MEMBER)]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}
