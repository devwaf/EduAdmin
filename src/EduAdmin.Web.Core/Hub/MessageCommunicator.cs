using Abp.Dependency;
using EduAdmin.Message;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Hub
{
    public class MessageCommunicator : IMessageCommunicator, ITransientDependency
    {
        private readonly IHubContext<MessageHub> _hubContent;
        public MessageCommunicator(IHubContext<MessageHub> hubContent)
        {
            _hubContent = hubContent;
        }
        /// <summary>
        /// 发送消息给所有客户端
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SendMessageToAll(string channel, string content)
        {
            await _hubContent.Clients.All.SendAsync(channel, content);
        }
    }
}
