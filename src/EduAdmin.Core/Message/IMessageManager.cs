using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Message
{
    public interface IMessageManager : IDomainService
    {
        Task BoradcastMessage(object obj);
        Task BoradcastNodeChangeCompanyId(Guid id);
    }
}
