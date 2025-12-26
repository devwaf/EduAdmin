using System.Threading.Tasks;
using Abp.Application.Services;
using EduAdmin.Authorization.Accounts.Dto;

namespace EduAdmin.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
