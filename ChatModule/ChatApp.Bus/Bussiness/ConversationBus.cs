using ChatApp.Bus.Interfaces;
using ChatApp.Dal.Interfaces;
using Common.Base;
using Models.Objects.Conversations;

namespace ChatApp.Bus.Bussiness
{
    public class ConversationBus : IConversationBus
    {
        private readonly IConversationRepo _conversation;
        public ConversationBus(IConversationRepo conversation)
        {
            _conversation = conversation;
        }
        public async Task<BaseResult<ConversationObj>> GetListConversation()
        {
            try
            {
                var result = await _conversation.GetListConversation();
                return result;
            }
            catch(Exception e)
            {
                return new BaseResult<ConversationObj>(false, e.Message);
            }
        }
        public async Task<BaseResult<ConversationObj>> GetConversationById(BaseRequest<GetConverationById> request)
        {
            try
            {
                var result = await _conversation.GetConversationById(request);
                return result;
            }
            catch(Exception e)
            {
                return new BaseResult<ConversationObj>(false, e.Message);
            }
        }
    }
}
