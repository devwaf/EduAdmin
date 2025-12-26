using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using EduAdmin.Authorization;
using EduAdmin.Authorization.Accounts;
using EduAdmin.Authorization.Roles;
using EduAdmin.Authorization.Users;
using EduAdmin.Entities;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using EduAdmin.MultiTenancy;
using EduAdmin.Roles.Dto;
using EduAdmin.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Role = EduAdmin.Authorization.Roles.Role;

namespace EduAdmin.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly TenantManager _tenantManager;
        private readonly IRepository<User, long> _userEFRepository;
        private readonly IRepository<Teacher, Guid> _teacherEFRepository;
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<StudentCourse, Guid> _studentCourseEFRepository;
        private readonly IRepository<ClassCourse, Guid> _classCourseEFRepository;
        private readonly string _defaulePassword = "123456";

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            TenantManager tenantManager,
            IRepository<User, long> userEFRepository,
            IRepository<Teacher, Guid> teacherEFRepository,
            IRepository<Student, Guid> studentEFRepository,
            IRepository<StudentCourse, Guid> studentCourseEFRepository,
            IRepository<ClassCourse, Guid> classCourseEFRepository,
            IPermissionManager permissionManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _userEFRepository = userEFRepository;
            _tenantManager = tenantManager;
            _teacherEFRepository = teacherEFRepository;
            _studentEFRepository = studentEFRepository;
            _permissionManager = permissionManager;
            _studentCourseEFRepository = studentCourseEFRepository;
            _classCourseEFRepository = classCourseEFRepository;
        }

        [AbpAllowAnonymous]//不需登录即可调用
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        { 
            //检测创建权限
           // CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

           // user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }
            
            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }
            
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }
        /// <summary>
        /// 通过手机号获取用户
        /// </summary>
        /// <param name="TenancyName"></param>
        /// <param name="PhoneNum"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]//不需登录即可调用
        public User GetUserByPhoneNumber(ref string TenancyName,string PhoneNum)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))//禁止数据过滤
            {
                var user = _userEFRepository.GetAll().FirstOrDefault(c =>c.IsActive && c.PhoneNumber == PhoneNum);
                if(user == null)
                {
                    return null;
                }
                //var tenant = _tenantManager.Tenants.FirstOrDefault(c => c.Id == user.TenantId);
                //if(tenant == null)
                //{
                //    throw new UserFriendlyException("该账户可能不存在！");
                //}
                //TenancyName = tenant?.TenancyName;
                return user;
            }
        }
        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await _userManager.ProhibitPermissionAsync(user, permission);
        }
        /// <summary>
        /// 获取用户的信息
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]//不需登录即可调用
        public async Task<SelectDto<Guid>> GetRoleInfo(long userId,string identity)
        {
            if(identity == "教师")
            {
              var userInfo =  await _teacherEFRepository.FirstOrDefaultAsync(c => c.UserId == userId);
                if(userInfo == null)
                {
                    throw new UserFriendlyException("输入的账号或密码有误");
                }
                return new SelectDto<Guid>(userInfo.Id,userInfo.Name);
            }
            if (identity == "学生")
            {
               var userInfo = await _studentEFRepository.FirstOrDefaultAsync(c => c.UserId == userId);
                if (userInfo == null)
                {
                    throw new UserFriendlyException("输入的账号或密码有误");
                }
                return new SelectDto<Guid>(userInfo.Id, userInfo.Name);
            }
            new UserFriendlyException("输入的账号或密码有误");
            return null;
        }
        [AbpAllowAnonymous]//不需登录即可调用
        public async Task<bool> PersonalRegister(RegisterDto input)
        {
            if (string.IsNullOrEmpty(input.Password))
                input.Password = _defaulePassword;
            string[] roleName = null;
            int tid;
            if(input.Role == "教师")
            {
                roleName = new string[] { "Teacher" };
                tid = 0;
            }else if(input.Role == "学生")
            {
                roleName = new string[] { "Student" };
                tid = 1;
            }
            else
            {
                new UserFriendlyException("不存在这个身份");
            }
            CreateUserDto creUser = new CreateUserDto()
            {
                Name = input.Name,
                UserName = "s" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                Surname = input.Name.Substring(0, 1),
                Password = input.Password,
                PhoneNumber = input.Phone,
                RoleNames = roleName,
                EmailAddress =LocalTool.GetTimeStamp(DateTime.Now).ToString(),
                TenantId = null,//默认为1，后续根据需求改变
                IsActive = true
            };
            var user = await CreateAsync(creUser);
            if(input.Role == "教师")
            {
                await _teacherEFRepository.InsertAsync(new Teacher
                {
                    UserId = user.Id,
                    JobNumber = input.Phone,
                    Name = user.Name,
                    IsDeleted = false,
                });
                await UpdateRolePermissions(new UpdateRolePermissionsInput
                {
                    RoleId =5,
                    GrantedPermissionNames = new List<string>
                    {
                        "Pages.Users"
                    }
                });
            }
            if(input.Role == "学生")
            {
                var stuId = await _studentEFRepository.InsertAndGetIdAsync(new Student
                {
                    UserId=user.Id,
                    Name = user.Name,
                    Sno = input.Phone,
                    ClassId = input.ClassId,
                    Gender = input.Gender,
                    IsDeleted=false,
                });
                var courseIds = await _classCourseEFRepository.GetAll().Where(c => c.ClassId == input.ClassId).Select(c => c.CourseId).ToListAsync(); 
                //学生注册时给学生绑定所在班级的所有课程
                foreach (var courseId in courseIds)
                {
                    await _studentCourseEFRepository.InsertAsync(new StudentCourse
                    {
                        StudentId = stuId,
                        CourseId = courseId
                    });
                }
                await UpdateRolePermissions(new UpdateRolePermissionsInput
                {
                    RoleId = 6,
                    GrantedPermissionNames = new List<string>
                    {
                        "Pages.Roles"
                    }
                });
            }
            return true;
        }
    }
}

