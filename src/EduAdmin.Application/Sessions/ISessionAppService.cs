using System.Threading.Tasks;
using Abp.Application.Services;
using EduAdmin.Sessions.Dto;

namespace EduAdmin.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
