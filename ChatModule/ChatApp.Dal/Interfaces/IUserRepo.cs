using Common.Base;
using Models.Objects.Friends;

namespace ChatApp.Dal.Interfaces
{
    public interface IUserRepo
    {
        public Task<BaseResult<FriendObj>> FindUser(BaseRequest<string> request);
    }
}
