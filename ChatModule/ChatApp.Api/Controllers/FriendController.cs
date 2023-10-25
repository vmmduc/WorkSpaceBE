using ChatApp.Bus.Interfaces;
using Common.Base;
using Microsoft.AspNetCore.Mvc;
using Models.Objects.Friend;
using Models.Objects.Friends;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendBus _friend;
        public FriendController(IFriendBus friend)
        {
            _friend = friend;
        }

        [HttpPost("GetListFriends")]
        public async Task<BaseResult<FriendObj>> GetListFriends()
        {
            var result = await _friend.GetListFriends();
            return result;
        }

        [HttpPost("MakeFriend")]
        public async Task<BaseResult<FriendObj>> MakeFriend(BaseRequest<MakeFriendObj> request)
        {
            var result = await _friend.MakeFriend(request);
            return result;
        }

        [HttpPost("ManageFriendship")]
        public async Task<BaseResult<FriendObj>> ManageFriendship(BaseRequest<ManageFriendShip> request)
        {
            var result = await _friend.ManageFriendship(request);
            return result;
        }
    }
}
