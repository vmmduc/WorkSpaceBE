using Common.Base;
using Models.Objects.Friend;
using Models.Objects.Friends;

namespace ChatApp.Bus.Interfaces
{
    public interface IFriendBus
    {
        public Task<BaseResult<FriendObj>> GetListFriends();
        public Task<BaseResult<FriendObj>> MakeFriend(BaseRequest<MakeFriendObj> request);
        public Task<BaseResult<FriendObj>> ManageFriendship(BaseRequest<ManageFriendShip> request);
    }
}
