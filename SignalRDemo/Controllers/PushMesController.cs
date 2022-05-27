using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRDemo.Hubs;
using System.Threading.Tasks;

namespace SignalRDemo.Controllers
{
    public class PushMesController : Controller
    {
        //注入IHubContext实例
        private readonly IHubContext<ChatHub> myHub;
        public ILogger<PushMesController> logger { get; set; }
        public PushMesController(IHubContext<ChatHub> _myHub)
        {
            myHub = _myHub;
        }
        /// <summary>
        /// 后端消息与会话-推送到PC端
        /// </summary>
        /// <returns></returns>
        [HttpPost("PushMes/MessagePushToSignalr")]
        public async Task<IActionResult> MessagePushAsync(SendMegSignalrDto message)
        {
            
             await myHub.Clients.All.SendAsync("ReceiveMessageToUId", "张三", message.msg.Txet);
            return Ok(new { code = "success", msg = "推送成功" });
        }
    }
}
