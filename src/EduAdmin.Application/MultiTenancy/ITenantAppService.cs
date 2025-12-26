using Abp.Application.Services;
using EduAdmin.MultiTenancy.Dto;

namespace EduAdmin.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

