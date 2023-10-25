using Common.Base;
using Models.Objects.Conversations;
using Models.Objects.Hubs;

namespace ChatApp.Dal.Interfaces
{
    public interface IConversationRepo
    {
        public Task<BaseResult<ConversationObj>> GetListConversation();
        public Task<BaseResult<ConversationObj>> GetConversationById(BaseRequest<GetConverationById> request);
        public Task<BaseResult<ConversationObj>> SendMessageToConversationId(BaseRequest<SendMessage> request);
        public Task<BaseResult<ConversationMemberObj>> GetConversationMember(int conversationId);
    }
}
