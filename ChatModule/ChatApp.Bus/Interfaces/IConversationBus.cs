using Common.Base;
using Models.Objects.Conversations;

namespace ChatApp.Bus.Interfaces
{
    public interface IConversationBus
    {
        public Task<BaseResult<ConversationObj>> GetListConversation();
        public Task<BaseResult<ConversationObj>> GetConversationById(BaseRequest<GetConverationById> request);
    }
}
