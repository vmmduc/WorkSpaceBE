using ChatApp.Bus.Interfaces;
using ChatApp.Dal.Interfaces;
using Common.Base;
using Models.Objects.Friends;

namespace ChatApp.Bus.Bussiness
{
    public class UserBus : IUserBus
    {
        private readonly IUserRepo _user;
        public UserBus(IUserRepo user) { _user = user; }
        public async Task<BaseResult<FriendObj>> FindUser(BaseRequest<string> request)
        {
            try
            {
                var result = await _user.FindUser(request);
                return result;
            }
            catch (Exception e)
            {
                return new BaseResult<FriendObj> (false, e.Message);
            }
        }
    }
}
