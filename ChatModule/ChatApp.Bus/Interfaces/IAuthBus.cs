using Common.Base;
using Models.Objects;

namespace ChatApp.Bus.Interfaces
{
    public interface IAuthBus
    {
        public Task<BaseResult<UserObj>> Login(BaseRequest<LoginObj> param);
    }
}
