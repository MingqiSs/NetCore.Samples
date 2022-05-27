using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SignalRDemo
{
    public class ChatMessage
    {
        private readonly ConcurrentDictionary<string, User> _msgs = new ConcurrentDictionary<string, User>();
        private IHubContext<ChatHub> Hub
        {
            get;
            set;
        }
        public ChatMessage(IHubContext<ChatHub> hub)
        {
            Hub = hub;
            LoadDefaultMsg();
        }
      
        private void LoadDefaultMsg()
        {
            _msgs.Clear();

            var stocks = new List<User>
            {
                new User { Name = "张三", HeadImg = "",Txet="这是默认对话1",SendTime=DateTime.Now.ToLongTimeString()},
                new User { Name = "李四", HeadImg = "",Txet="这是默认对话1",SendTime=DateTime.Now.ToLongTimeString()},
                new User { Name = "王五", HeadImg = "",Txet="这是默认对话1",SendTime=DateTime.Now.ToLongTimeString()}
            };

            stocks.ForEach(stock => _msgs.TryAdd(stock.Name, stock));
        }
        public IEnumerable<User> GetAllMsgs()
        {
            return _msgs.Values;
        }
    }
}
