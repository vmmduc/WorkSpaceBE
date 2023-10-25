using Common.Base;
using Models.Objects.Friends;

namespace ChatApp.Bus.Interfaces
{
    public interface IUserBus
    {
        public Task<BaseResult<FriendObj>> FindUser(BaseRequest<string> request);
    }
}
