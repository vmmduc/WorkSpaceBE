using ChatApp.Dal.Interfaces;
using Common.Base;
using Common.Extentions;
using Common.HttpContextAccessor;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Objects.Friends;

namespace ChatApp.Dal.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly DatabaseContext _context;
        private readonly ICurrentUserService _currentUser;
        public UserRepo(DatabaseContext context, ICurrentUserService currenUser)
        {
            _context = context;
            _currentUser = currenUser;
        }

        public async Task<BaseResult<FriendObj>> FindUser(BaseRequest<string> request)
        {
            var currentUserId = _currentUser.UserId;
            var lsFriendsShip = await _context.LS_FRIENDSHIPS.Where(x => x.FK_USER_ID == currentUserId || x.FK_FRIEND_ID == currentUserId)
                                      .Join(_context.LS_STATES,
                                      friendShip => friendShip.FK_STATE_ID,
                                      state => state.PK_STATE_ID,
                                      (friendShip, state) => new
                                      {
                                          friendshipId = friendShip.PK_FRIENDSHIP_ID,
                                          userId = friendShip.FK_USER_ID,
                                          friendId = friendShip.FK_FRIEND_ID,
                                          stateCode = state.STATE_CODE,
                                          isSendRequest = friendShip.FK_USER_ID == _currentUser.UserId // trả về true nếu currentUser gửi yêu cầu kết bạn
                                      }).ToListAsync();
            var lsUser = await _context.LS_USERS.Where(x => x.PK_USER_ID != currentUserId && x.EMAIL!.Trim().ToLower().Contains(request.Param!.Trim().ToLower())).ToListAsync();
            var dataList = lsUser.Select(x => new FriendObj
            {
                PK_USER_ID = x.PK_USER_ID,
                EMAIL = x.EMAIL,
                FULL_NAME = x.FULL_NAME,
                StateCode = lsFriendsShip.FirstOrDefault(y => (y.userId == _currentUser.UserId && y.friendId == x.PK_USER_ID)
                                                  || (y.userId == x.PK_USER_ID && y.friendId == _currentUser.UserId)).GetValue(y => y.stateCode, null),

                IsSendRequest = lsFriendsShip.FirstOrDefault(y => (y.userId == _currentUser.UserId && y.friendId == x.PK_USER_ID)
                                                  || (y.userId == x.PK_USER_ID && y.friendId == _currentUser.UserId)).GetValue(y => y.isSendRequest, false),

                FriendShipId = lsFriendsShip.FirstOrDefault(y => (y.userId == _currentUser.UserId && y.friendId == x.PK_USER_ID)
                                                  || (y.userId == x.PK_USER_ID && y.friendId == _currentUser.UserId)).GetValue(y => y.friendshipId, 0)
            }).ToList();
            dataList = dataList.OrderByDescending(x => x.StateCode).ThenByDescending(x => x.FULL_NAME).ToList();
            return new BaseResult<FriendObj> { rtList = dataList, rtCode = 0 };
        }
    
        
    }
}