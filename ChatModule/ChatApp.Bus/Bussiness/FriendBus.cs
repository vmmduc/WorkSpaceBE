using ChatApp.Bus.Interfaces;
using ChatApp.Dal.Interfaces;
using Common.Base;
using Models.Objects.Friend;
using Models.Objects.Friends;

namespace ChatApp.Bus.Bussiness
{
    public class FriendBus : IFriendBus
    {
        private readonly IFriendRepo _friend;
        public FriendBus(IFriendRepo friend)
        {
            _friend = friend;
        }

        public async Task<BaseResult<FriendObj>> GetListFriends()
        {
            try
            {
                var result = await _friend.GetListFriends();
                return result;
            }
            catch (Exception e)
            {
                return new BaseResult<FriendObj> (false, e.Message);
            }
        }

        public async Task<BaseResult<FriendObj>> MakeFriend(BaseRequest<MakeFriendObj> request)
        {
            try
            {
                var result = await _friend.MakeFriend(request);
                return result;
            }
            catch (Exception e)
            {
                return new BaseResult<FriendObj>(false, e.Message);
            }
        }

        public async Task<BaseResult<FriendObj>> ManageFriendship(BaseRequest<ManageFriendShip> request)
        {
            try
            {
                var result = await _friend.ManageFriendship(request);
                return result;
            }
            catch (Exception e)
            {
                return new BaseResult<FriendObj>(false, e.Message);
            }
        }
    }
}
