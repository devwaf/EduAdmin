using Abp.AspNetCore.SignalR.Hubs;
using Abp.RealTime;
using EduAdmin.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Hub
{
    public class MessageHub : OnlineClientHubBase
    {
        private readonly IMessageManager _messageManager;
        public MessageHub(IMessageManager messageManager,
            IOnlineClientManager onlineClientManager,
            IOnlineClientInfoProvider clientInfoProvider) : base(onlineClientManager, clientInfoProvider)
        {
            _messageManager = messageManager;
        }
        //消息广播
        public async Task Broadcast(string message)
        {
            //await Clients.All.SendAsync(message);
            await _messageManager.BoradcastMessage(message);
        }
    }
}
