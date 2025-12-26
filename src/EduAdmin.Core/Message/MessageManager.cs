using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Message
{
    public class MessageManager : DomainService, IMessageManager
    {
        private readonly IMessageCommunicator _messageCommunicator;
        public MessageManager(IMessageCommunicator messageCommunicator)
        {
            _messageCommunicator = messageCommunicator;
        }
        public async Task BoradcastMessage(object obj)
        {
            string content = obj.ToString();
            await _messageCommunicator.SendMessageToAll("broadcast", content);
        }

        public async Task BoradcastNodeChangeCompanyId(Guid id)
        {
            await _messageCommunicator.SendMessageToAll("NodeChangeCompanyId", id.ToString());
        }
    }
}
