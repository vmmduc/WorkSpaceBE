using ChatApp.Dal.Interfaces;
using Common.Base;
using Common.Constant;
using Common.Extentions;
using Common.HttpContextAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Models.Models;
using Models.Objects.Friend;
using Models.Objects.Friends;

namespace ChatApp.Dal.Repositories
{
    public class FriendRepo : IFriendRepo
    {
        private readonly DatabaseContext _context;
        private readonly ICurrentUserService _currenUser;
        public FriendRepo(DatabaseContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currenUser = currentUser;
        }
        // current user
        public async Task<BaseResult<FriendObj>> GetListFriends()
        {
            var lsFriends = await _context.LS_FRIENDSHIPS.Where(x => x.FK_USER_ID == _currenUser.UserId || x.FK_FRIEND_ID == _currenUser.UserId)
                            .Join(_context.LS_STATES,
                                  friend_ship => friend_ship.FK_STATE_ID,
                                  state => state.PK_STATE_ID,
                                  (friend_ship, state) => new
                                  {
                                      friend_ship_id = friend_ship.PK_FRIENDSHIP_ID,
                                      fkUserId = friend_ship.FK_USER_ID,
                                      fkFriendId = friend_ship.FK_FRIEND_ID,
                                      select_friend_id = friend_ship.FK_USER_ID == _currenUser.UserId ? friend_ship.FK_FRIEND_ID : friend_ship.FK_USER_ID,
                                      stateCode = state.STATE_CODE
                                  }).Where(x => x.stateCode == State.Accept || (x.fkFriendId == _currenUser.UserId && x.stateCode == State.WaitingConfirm))
                            .Join(_context.LS_USERS,
                                  x => x.select_friend_id,
                                  y => y.PK_USER_ID,
                                  (x, y) => new FriendObj
                                  {
                                      PK_USER_ID = x.fkFriendId,
                                      EMAIL = y.EMAIL,
                                      FULL_NAME = y.FULL_NAME,
                                      PHONE = y.PHONE_NUMBER,
                                      StateCode = x.stateCode,
                                      FriendShipId = x.friend_ship_id
                                  })
                            .ToListAsync();
            lsFriends = lsFriends.OrderByDescending(x => x.StateCode == State.WaitingConfirm ? 1 : 0).ThenByDescending(x => x.FULL_NAME).ToList();
            return new BaseResult<FriendObj> {rtList = lsFriends, rtCode = 0 };
        }

        // currentUser
        public async Task<BaseResult<FriendObj>> MakeFriend(BaseRequest<MakeFriendObj> request)
        {
            try
            {
                var user = await _context.LS_USERS.FirstOrDefaultAsync(x => x.PK_USER_ID == request.Param.FriendId);
                if (user == null) return new BaseResult<FriendObj>(false, "UserId không tồn tại");
                var friendShip = await _context.LS_FRIENDSHIPS
                            .Where(x => ((x.FK_USER_ID == _currenUser.UserId && x.FK_FRIEND_ID == request.Param.FriendId)
                                      || (x.FK_USER_ID == request.Param.FriendId && x.FK_FRIEND_ID == _currenUser.UserId))
                                      ).FirstOrDefaultAsync();
                var friendShipId = 0;
                var pkStateId = _context.LS_STATES.FirstOrDefault(x => x.STATE_CODE == State.WaitingConfirm).GetValue(x => x.PK_STATE_ID, 0);
                if (friendShip == null)
                {
                    FRIENDSHIP newFriendship = new FRIENDSHIP
                    {
                        FK_USER_ID = _currenUser.UserId,
                        FK_FRIEND_ID = request.Param.FriendId,
                        FK_STATE_ID = pkStateId
                    };
                    await _context.LS_FRIENDSHIPS.AddAsync(newFriendship);
                    await _context.SaveChangesAsync();
                    friendShipId = newFriendship.PK_FRIENDSHIP_ID;
                }
                else
                {
                    friendShip.FK_STATE_ID = pkStateId;
                    friendShip.FK_USER_ID = _currenUser.UserId;
                    friendShip.FK_FRIEND_ID = request.Param.FriendId;
                    friendShipId = friendShip.PK_FRIENDSHIP_ID;
                    await _context.SaveChangesAsync();
                }

                var rtData = new FriendObj
                {
                    PK_USER_ID = request.Param.FriendId,
                    EMAIL = user.EMAIL,
                    FULL_NAME = user.FULL_NAME,
                    PHONE = user.PHONE_NUMBER,
                    StateCode = State.WaitingConfirm,
                    IsSendRequest = true,
                    FriendShipId = friendShipId
                };

                return new BaseResult<FriendObj> { rtResult = rtData, rtCode = 0 };
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException(e.Message);
            }
        }

        public async Task<BaseResult<FriendObj>> ManageFriendship(BaseRequest<ManageFriendShip> request)
        {
            try
            {
                var friendShip = await _context.LS_FRIENDSHIPS.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PK_FRIENDSHIP_ID == request.Param.PK_FRIEND_SHIP);
                if (friendShip == null) return new BaseResult<FriendObj>(false, "Không tìm thất bản ghi!");

                var lsState = await _context.LS_STATES.ToListAsync();
                int stateId = 0;
                switch ((FriendAction)request.Param.friendAction)
                {
                    case FriendAction.ACCEPT:
                        {
                            stateId = lsState.FirstOrDefault(x => x.STATE_CODE == State.Accept).GetValue(x => x.PK_STATE_ID, 0);
                            break;
                        }
                    case FriendAction.REJECT:
                        {
                            stateId = lsState.FirstOrDefault(x => x.STATE_CODE == State.Reject).GetValue(x => x.PK_STATE_ID, 0);
                            break;
                        }
                    case FriendAction.CANCEL:
                        {
                            stateId = lsState.FirstOrDefault(x => x.STATE_CODE == State.Cancel).GetValue(x => x.PK_STATE_ID, 0);
                            break;
                        }
                    default:
                        {
                            stateId = 0;
                            break;
                        }
                }
                if (stateId == 0) return new BaseResult<FriendObj>(false, "!");

                friendShip.FK_STATE_ID = stateId;
                _context.Update(friendShip);
                await _context.SaveChangesAsync();

                return new BaseResult<FriendObj>
                {
                    rtResult = new FriendObj
                    {
                        StateCode = lsState.FirstOrDefault(x => x.PK_STATE_ID == stateId).GetValue(x => x.STATE_CODE, null),
                        IsSendRequest = friendShip.FK_USER_ID == _currenUser.UserId
                    },
                    rtCode = 0
                };
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException(e.Message);
            }
        }
    }
}
