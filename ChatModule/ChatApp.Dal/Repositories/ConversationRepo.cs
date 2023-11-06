using ChatApp.Dal.Interfaces;
using Common.Base;
using Common.HttpContextAccessor;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Objects.Conversations;
using Models.Objects.Hubs;

namespace ChatApp.Dal.Repositories
{
    public class ConversationRepo : IConversationRepo
    {
        private readonly DatabaseContext _context;
        private readonly ICurrentUserService _currenUser;
        public ConversationRepo(DatabaseContext context, ICurrentUserService currenUser)
        {
            _context = context;
            _currenUser = currenUser;
        }

        public async Task<BaseResult<ConversationObj>> GetListConversation()
        {
            // Xử lý unread message
            var userId = _currenUser.UserId;
            var dataList = await _context.LS_CONVERSATION_MEMBERS.AsQueryable().Where(x => x.FK_USER_ID == userId)
                             .Join(_context.LS_CONVERSATIONS,
                                   conv_mem => conv_mem.FK_CONVERSATION_ID,
                                   conv => conv.PK_CONVERSATION_ID,
                                   (conv_mem, conv) => new { conv_mem, conv })
                             .Select(x => new ConversationObj
                             {
                                 conversationId = x.conv.PK_CONVERSATION_ID,
                                 isGroup = x.conv.IS_GROUP ?? false,
                                 displayName = x.conv.IS_GROUP == true ?
                                               x.conv.CONVERSATION_NAME :
                                               _context.LS_CONVERSATION_MEMBERS
                                               .Where(y => y.FK_CONVERSATION_ID == x.conv.PK_CONVERSATION_ID && y.FK_USER_ID != userId)
                                               .Join(_context.LS_USERS,
                                                     a => a.FK_USER_ID,
                                                     b => b.PK_USER_ID,
                                                     (a, b) => string.IsNullOrEmpty(a.PSEUDONYM) ? b.FULL_NAME : a.PSEUDONYM)
                                               .FirstOrDefault(),
                                 message = _context.LS_MESSAGES
                                                  .Where(y => y.FK_CONVERSATION_ID == x.conv.PK_CONVERSATION_ID)
                                                  .OrderByDescending(y => y.CREATED_DATE)
                                                  .Join(_context.LS_USERS,
                                                        message => message.FK_SENDER_ID,
                                                        user => user.PK_USER_ID,
                                                        (message, user) => new { message, user })
                                                  .Select(y => new MessageObj
                                                  {
                                                      messageContent = y.message.MESSAGE_CONTENT,
                                                      createdDate = y.message.CREATED_DATE
                                                  }).FirstOrDefault(),
                                 unreadMessageCount = x.conv_mem.UNREAD_COUNT ?? 0,
                                 lastUpdatedDate = x.conv.LAST_UPDATED_DATE
                             }).ToListAsync();
            if (dataList.Count > 0)
                dataList = dataList.OrderByDescending(x => x.lastUpdatedDate).ToList();
            return new BaseResult<ConversationObj>(dataList, true, 0);
        }

        public async Task<BaseResult<ConversationObj>> GetConversationById(BaseRequest<GetConverationById> request)
        {
            // Lấy danh sách tin nhắn
            var lsMsg = await _context.LS_CONVERSATIONS.Where(x => x.PK_CONVERSATION_ID == request.Param.conversationId)
                                  .Join(_context.LS_MESSAGES,
                                        conversation => conversation.PK_CONVERSATION_ID,
                                        message => message.FK_CONVERSATION_ID,
                                        (conversation, message) => new MessageObj
                                        {
                                            conversationId = conversation.PK_CONVERSATION_ID,
                                            messageId = message.PK_MESSAGE_ID,
                                            messageContent = message.MESSAGE_CONTENT,
                                            senderId = message.FK_SENDER_ID,
                                            createdDate = message.CREATED_DATE,
                                            groupId = message.GROUP_ID ?? 0
                                        }).ToListAsync();
            lsMsg = lsMsg.OrderByDescending(x => x.createdDate)
                                 .Skip((request.Param.pageIndex - 1) * request.Param.pageSize)
                                 .Take(request.Param.pageSize)
                                 .Reverse()
                                 .ToList();

            // Lấy danh sách thành viên trong cuộc trò chuyện
            var lsMember = await _context.LS_CONVERSATION_MEMBERS.AsQueryable().Where(x => x.FK_CONVERSATION_ID == request.Param.conversationId)
                           .Join(_context.LS_USERS,
                                 conversationMember => conversationMember.FK_USER_ID,
                                 user => user.PK_USER_ID,
                                 (conversationMember, user) => new ConversationMemberObj
                                 {
                                     userId = user.PK_USER_ID,
                                     fullName = user.FULL_NAME,
                                     pseudonym = string.IsNullOrEmpty(conversationMember.PSEUDONYM) ? user.FULL_NAME : conversationMember.PSEUDONYM
                                 }).ToListAsync();
            var conversation = new ConversationObj
            {
                conversationId = request.Param.conversationId,
                lsMessage = lsMsg,
                conversationMembers = lsMember,
            };
            return new BaseResult<ConversationObj>(conversation, true, 0);
        }

        public async Task<BaseResult<ConversationMemberObj>> GetConversationMember(int conversationId)
        {
            var message = _context.LS_MESSAGES.Where(x => x.FK_CONVERSATION_ID == conversationId)
                                .OrderByDescending(x => x.CREATED_DATE).FirstOrDefault();

            var lsMember = await _context.LS_CONVERSATION_MEMBERS.AsQueryable()
                .Where(x => x.FK_CONVERSATION_ID == conversationId)
                .Select(x => new ConversationMemberObj
                {
                    userId = x.FK_USER_ID,
                    unreadMessageCount = x.UNREAD_COUNT ?? 0,
                    newMessage = message == null ? "" : message.MESSAGE_CONTENT ?? "",
                    conversationId = x.FK_CONVERSATION_ID
                }).ToListAsync();
            return new BaseResult<ConversationMemberObj>(lsMember, true, 0);
        }

        public async Task<BaseResult<ConversationObj>> SendMessageToConversationId(BaseRequest<SendMessage> request)
        {
            var conv = await _context.LS_CONVERSATIONS.FirstOrDefaultAsync(x => x.PK_CONVERSATION_ID == request.Param.conversationId);
            
            if (conv == null) return new BaseResult<ConversationObj>(false, "");
            // Lưu tin nhắn mới vào database
            var message = new MESSAGE
            {
                FK_SENDER_ID = request.Param.senderId,
                FK_CONVERSATION_ID = request.Param.conversationId,
                MESSAGE_CONTENT = request.Param.messageContent,
                CREATED_DATE = DateTime.Now,
                GROUP_ID = request.Param.groupId
            };

            await _context.LS_MESSAGES.AddAsync(message);
            await _context.SaveChangesAsync();

            conv.LAST_UPDATED_DATE = DateTime.Now;
            await _context.SaveChangesAsync();

            // Cập nhật tin nhắn chưa đọc cho user
            var lsMember = await _context.LS_CONVERSATION_MEMBERS.AsNoTracking()
                                 .Where(x => x.FK_CONVERSATION_ID == request.Param.conversationId
                                        && x.FK_USER_ID != message.FK_SENDER_ID
                                        && x.IS_DELETED != true)
                                 .ToListAsync();

            lsMember.ForEach(x => x.UNREAD_COUNT += 1);
            _context.UpdateRange(lsMember);
            await _context.SaveChangesAsync();

            // Lấy danh sách user để phát thông báo
            var conversationMembers = _context.LS_CONVERSATION_MEMBERS
                                       .Join(_context.LS_USERS,
                                             member => member.FK_USER_ID,
                                             user => user.PK_USER_ID,
                                             (member, user) => new ConversationMemberObj
                                             {
                                                 userId = member.FK_USER_ID,
                                                 newMessage = message.MESSAGE_CONTENT,
                                                 unreadMessageCount = member.UNREAD_COUNT,
                                                 conversationId = message.FK_CONVERSATION_ID,
                                                 fullName = user.FULL_NAME,
                                                 pseudonym = member.PSEUDONYM
                                             }).ToList();

            // trả về tin nhắn vừa gửi
            var conversation = new ConversationObj
            {
                conversationId = message.FK_CONVERSATION_ID,
                isGroup = conv.IS_GROUP,
                displayName = conv.CONVERSATION_NAME,
                message = new MessageObj
                {
                    conversationId = message.FK_CONVERSATION_ID,
                    messageId = message.PK_MESSAGE_ID,
                    senderId = message.FK_SENDER_ID,
                    messageContent = message.MESSAGE_CONTENT,
                    createdDate = message.CREATED_DATE,
                    groupId = message.GROUP_ID ?? 0
                },
                conversationMembers = conversationMembers
            };
            return new BaseResult<ConversationObj>(conversation, true, 0);
        }
        #region SendMessageToUser
        /*public async Task SendMessageToUser(BaseRequest<Message> request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var conversation = await _context.LS_CONVERSATION_MEMBERS
                                     .Where(x => x.FK_USER_ID == _currenUser.UserId || x.FK_USER_ID == request.Param!.RECEIVER_ID)
                                     .GroupBy(x => x.FK_USER_ID)
                                     .Select(group => new
                                     {
                                         FK_USER_ID = group.Key,
                                         seq = group.Count(),
                                         conversationId = group.Select(x => x.FK_CONVERSATION_ID).FirstOrDefault(),
                                     })
                                     .Join(_context.LS_CONVERSATIONS,
                                           conv_mem => conv_mem.conversationId,
                                           conv => conv.PK_CONVERSATION_ID,
                                           (conv_mem, conv) => new { conv_mem, conv })
                                     .Where(x => x.conv_mem.FK_USER_ID != _currenUser.UserId && x.conv_mem.seq == 2 && x.conv.IS_GROUP == false)
                                     .Select(x => new { conversationId = x.conv.PK_CONVERSATION_ID })
                                     .FirstOrDefaultAsync();
            // nếu conversation != null thì insert message vào conversation đó
            if (conversation != null)
            {
                await _context.LS_MESSAGES.AddAsync(new MESSAGE
                {
                    FK_SENDER_ID = _currenUser.UserId,
                    FK_CONVERSATION_ID = conversation.conversationId,
                    CREATED_DATE = DateTime.UtcNow,
                    IS_READ = false,
                    MESSAGE_CONTENT = request.Param!.MESSAGE_CONTENT
                });
                await _context.SaveChangesAsync();
            }
            else
            {
                // nếu conversation == null thì tạo 1 conversation mới
                var conv = new CONVERSATION
                {
                    CONVERSATION_NAME = null,
                    IS_GROUP = false,
                };
                await _context.LS_CONVERSATIONS.AddAsync(conv);
                await _context.SaveChangesAsync();

                // thêm message
                await _context.LS_MESSAGES.AddAsync(new MESSAGE
                {
                    FK_SENDER_ID = _currenUser.UserId,
                    FK_CONVERSATION_ID = conv.PK_CONVERSATION_ID,
                    CREATED_DATE = DateTime.UtcNow,
                    IS_READ = false,
                    MESSAGE_CONTENT = request.Param!.MESSAGE_CONTENT
                });
                await _context.SaveChangesAsync();

                // thêm member
                var lsConv = new List<CONVERSATION_MEMBER>
                {
                    //new CONVERSATION_MEMBER(request.Param.SENDER_ID, conv.PK_CONVERSATION_ID),
                    //new CONVERSATION_MEMBER(request.Param.RECEIVER_ID, conv.PK_CONVERSATION_ID),
                };
                await _context.AddAsync(lsConv);
                await _context.SaveChangesAsync();
            }
            transaction.Commit();
        }*/
        #endregion
    }
}
