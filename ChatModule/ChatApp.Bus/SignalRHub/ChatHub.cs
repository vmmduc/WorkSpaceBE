using ChatApp.Dal.Interfaces;
using Common.Base;
using Microsoft.AspNetCore.SignalR;
using Models.Objects.Conversations;
using Models.Objects.Hubs;
using Newtonsoft.Json;

namespace ChatApp.Bus.SignalRHub
{
    public class ChatHub : Hub
    {
        private static List<UserConnectionHub> _connectingUser = new List<UserConnectionHub>();
        private readonly IConversationRepo _conversation;
        public ChatHub(IConversationRepo conversation)
        {
            _conversation = conversation;
        }

        public void SetUserOnline(int userId)
        {
            if(userId != 0)
            {
                _connectingUser.Add(new UserConnectionHub { USER_ID = userId, ConnectionId = Context.ConnectionId });
            }
        }

        public async Task SendMessageToConversationId(BaseRequest<SendMessage> request)
        {
            try
            {
                var response = await _conversation.SendMessageToConversationId(request);
                if (response.rtStatus)
                {
                    var lsUser = _connectingUser.Where(x => response.rtResult!.conversationMembers.Select(y=>y.userId).Contains(x.USER_ID)).ToList();
                    foreach (var item in lsUser)
                        await Clients.Client(item.ConnectionId).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(response.rtResult));
                }
            }
            catch { }
        }

        public async Task PreparingMessage(BaseRequest<PreparingMessageObj> request)
        {
            try
            {
                var response = await _conversation.GetConversationMember(request.Param.conversationId);
                if (response.rtStatus)
                {
                    var lsUser = _connectingUser.Where(x => response.rtList.Select(x => x.userId).Contains(x.USER_ID) && x.USER_ID != request.Param.senderId).ToList();
                    foreach (var item in lsUser)
                        await Clients.Client(item.ConnectionId).SendAsync("ReceivePreparingMessage", request.Param.status);
                }
            }
            catch { }
        }
        #region override
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionItem = _connectingUser.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (connectionItem != null) _connectingUser.Remove(connectionItem);
            return base.OnDisconnectedAsync(exception);
        }
        public override Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Headers["Authorization"];
            return base.OnConnectedAsync();
        }
        #endregion
    }
}
