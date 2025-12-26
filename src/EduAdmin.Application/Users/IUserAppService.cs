using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EduAdmin.Authorization.Users;
using EduAdmin.LocalTools.Dto;
using EduAdmin.Roles.Dto;
using EduAdmin.Users.Dto;

namespace EduAdmin.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task DeActivate(EntityDto<long> user);
        Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        Task ChangeLanguage(ChangeUserLanguageDto input);
        Task<bool> ChangePassword(ChangePasswordDto input);
        User GetUserByPhoneNumber(ref string TenancyName,string PhoneNum);
        Task<UserDto> CreateAsync(CreateUserDto input);
        Task<SelectDto<Guid>> GetRoleInfo(long userId, string identity);
        Task<bool> PersonalRegister(RegisterDto input);
    }
}
