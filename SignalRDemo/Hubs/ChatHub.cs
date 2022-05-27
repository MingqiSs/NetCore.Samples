using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SignalRDemo.Token;

namespace SignalRDemo.Hubs
{
    //不需要token demo
    public class ChatHub:Hub
    {
        // private readonly ChatMessage  _chatMessage;
        protected static List<User> userInfoList = new List<User>();
        public ChatHub()
        {
           // _chatMessage = chatMessage;
        }
        public async Task SendMessage(string user, string message)
        {
            var u = userInfoList.Where(q => q.ConnectionId != Context.ConnectionId).FirstOrDefault();
            if (u != null)
            {
                await Clients.Client(u.ConnectionId).SendAsync("broadcastMessage", user,$"服务端返回- {Context.ConnectionId}:{message}");
            }
        }
        public override async Task OnConnectedAsync()
        {
            if (!userInfoList.Exists(q => q.ConnectionId == Context.ConnectionId))
            {
                userInfoList.Add(new User { ConnectionId = Context.ConnectionId, HeadImg = "tst", Name = string.Empty });
            }
            await Clients.All.SendAsync("ConnectedResult", "欢迎进入聊天室!");
            await base.OnConnectedAsync();
        }

    }
}
