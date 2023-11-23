using Common.Base;
using Models.Models;
using Models.Objects;

namespace ChatApp.Dal.Interfaces
{
    public interface IAuthRepo
    {
        public Task<BaseResult<UserObj>> Login(BaseRequest<LoginObj> param);
        public Task CreateRefreshToken(RefreshTokenObj param);
    }
}
