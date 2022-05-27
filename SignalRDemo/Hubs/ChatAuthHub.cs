using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SignalRDemo.Token;
using Microsoft.AspNetCore.Authorization;

namespace SignalRDemo.Hubs
{
    [Authorize]
    public class ChatAuthHub : Hub
    {
        protected static List<User> userInfoList = new List<User>();
        private static string GroupName = "GroupDemo";
        public ChatAuthHub()
        {
            // _chatMessage = chatMessage;
        }
        public async Task SendMessage(string user, string message)
        {
            var current = Context.User.GetToken();
            var u = userInfoList.Where(q => q.UID == current.UID).FirstOrDefault();
            if (u != null)
            {
                //测试私聊
                await Clients.Users(u.UID).SendAsync("ConnectedResult", $"服务端返回- {u.UID}:{message}");

                //测试群聊
                await SendMessageByGroup(GroupName, message);
            }
        }
        public async Task SendMessageByGroup(string groupName, string message)
        {
            var current = Context.User.GetToken();
            var u = userInfoList.Where(q => q.UID == current.UID).FirstOrDefault();
            if (u != null)
            {
                await Clients.Group(groupName).SendAsync("ConnectedResult", $"服务端返回群聊- {groupName}:{message}");
            }
        }
        /// <summary>
        /// 添加群聊
        /// </summary>
        /// <returns></returns>
        public async Task AddGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);
        }
        /// <summary>
        /// 连接/重连
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var user = Context.User.GetToken();

            if (!userInfoList.Exists(q => q.ConnectionId == Context.ConnectionId))
            {
                userInfoList.Add(new User { UID = user.UID, ConnectionId = Context.ConnectionId, HeadImg = "tst", Name = user.Name }) ;
                //测试连接添加分组
               await AddGroup();
            }
            /// todo Users(UID)对应token中的ClaimTypes.NameIdentifier项，需要颁发token时 把UID赋值给ClaimTypes.NameIdentifier，
            /// new Claim(ClaimTypes.NameIdentifier, userToken.UID.ToString()),
            await Clients.Users(user.UID).SendAsync("ConnectedResult", $"欢迎进入聊天室! {user.Name}");

            await base.OnConnectedAsync();
        }
    }
}
