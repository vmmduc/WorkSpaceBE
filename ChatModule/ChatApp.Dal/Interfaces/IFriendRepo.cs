using Common.Base;
using Models.Objects.Friend;
using Models.Objects.Friends;

namespace ChatApp.Dal.Interfaces
{
    public interface IFriendRepo
    {
        public Task<BaseResult<FriendObj>> GetListFriends();
        public Task<BaseResult<FriendObj>> MakeFriend(BaseRequest<MakeFriendObj> request);
        public Task<BaseResult<FriendObj>> ManageFriendship(BaseRequest<ManageFriendShip> request);
    }
}
