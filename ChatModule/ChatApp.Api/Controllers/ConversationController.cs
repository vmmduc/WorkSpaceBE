using ChatApp.Bus.Interfaces;
using Common.Base;
using Microsoft.AspNetCore.Mvc;
using Models.Objects.Conversations;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationBus _conversation;
        public ConversationController(IConversationBus conversation)
        {
            _conversation = conversation;
        }
        [HttpPost("GetListConversation")]
        public async Task<BaseResult<ConversationObj>> GetListConversation()
        {
            var result = await _conversation.GetListConversation();
            return result;
        }

        [HttpPost("GetConversationById")]
        public async Task<BaseResult<ConversationObj>> GetConversationById(BaseRequest<GetConverationById> request)
        {
            var result = await _conversation.GetConversationById(request);
            return result;
        }
    }
}
