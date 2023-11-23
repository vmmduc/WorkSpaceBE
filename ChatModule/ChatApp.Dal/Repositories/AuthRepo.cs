using ChatApp.Dal.Interfaces;
using Common.Base;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Objects;

namespace ChatApp.Dal.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DatabaseContext _context;
        public AuthRepo(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<BaseResult<UserObj>> Login(BaseRequest<LoginObj> param)
        {
            var result = await _context.LS_USERS.Where(x => x.EMAIL == param.Param.UserName && x.PASSWORD == param.Param.Password)
                                        .Select(x => new UserObj
                                        {
                                            userId = x.PK_USER_ID,
                                            email = x.EMAIL,
                                            fullName = x.FULL_NAME,
                                            phoneNumber = x.PHONE_NUMBER,
                                            deactivate = x.DEACTIVATE,
                                            lsRoles = _context.LS_USER_ROLES.Where(y => y.FK_USER_ID == x.PK_USER_ID)
                                                              .Join(_context.LS_ROLES,
                                                                    user_role => user_role.FK_ROLE_ID,
                                                                    role => role.PK_ROLE_ID,
                                                                    (user_role, role) => new RoleObj
                                                                    {
                                                                        RoleId = role.PK_ROLE_ID,
                                                                        RoleCode = role.ROLE_CODE,
                                                                        RoleName = role.ROLE_NAME
                                                                    }).ToList()
                                        }).FirstOrDefaultAsync();

            if (result == null) return new BaseResult<UserObj> { rtMessage = "Email hoặc mật khẩu không đúng", rtStatus = false, rtCode = -1 };
            return new BaseResult<UserObj> { rtResult = result, rtStatus = true, rtCode = 0 };
        }

        public async Task CreateRefreshToken(RefreshTokenObj param)
        {
            await _context.LS_USER_TOKEN.AddAsync(new USER_TOKEN
            {
                FK_USER_ID = param.UserId,
                REFRESH_TOKEN = param.RefreshToken,
                CREATED_DATE = DateTime.Now,
                EXPIRES_TIME = param.ExpiresTime,
                IS_REVOKED = false
            });
            await _context.SaveChangesAsync();
        }
    }
}
