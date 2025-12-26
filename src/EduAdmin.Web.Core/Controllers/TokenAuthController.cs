using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using EduAdmin.Authentication.External;
using EduAdmin.Authentication.JwtBearer;
using EduAdmin.Authorization;
using EduAdmin.Authorization.Users;
using EduAdmin.Models.TokenAuth;
using EduAdmin.MultiTenancy;
using EduAdmin.Users;
using EduAdmin.Users.Dto;

namespace EduAdmin.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : EduAdminControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IUserAppService _userAppService;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            IUserAppService userAppService)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _userAppService = userAppService;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            if(model.Identity != "教师" && model.Identity != "学生")
            {
                throw new UserFriendlyException("不存在这样的角色");
            }
            string TenancyName = "";
            var user = _userAppService.GetUserByPhoneNumber(ref TenancyName, model.PhoneNum);
            if (user == null)
            {
                throw new UserFriendlyException("该账号不存在");
            }
            var loginResult = await GetLoginResultAsync(
                user.UserName,
                model.Password,
                TenancyName
            );
            //获得令牌 写入Token
            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity,model.Identity));
            var userInfo = await _userAppService.GetRoleInfo(user.Id, model.Identity);
            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
                Name = userInfo.Label,
                UserInfoId = userInfo.Value
            };
        }
        //public async Task CreateUser(CreateUserDto input)
        //{
        //   await _userAppService.CreateAsync(input);
        //}
        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        //[HttpPost]
        //public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        //{
        //    var externalUser = await GetExternalUserInfo(model);

        //    var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

        //    switch (loginResult.Result)
        //    {
        //        case AbpLoginResultType.Success:
        //            {
        //                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
        //                return new ExternalAuthenticateResultModel
        //                {
        //                    AccessToken = accessToken,
        //                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
        //                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
        //                };
        //            }
        //        case AbpLoginResultType.UnknownExternalLogin:
        //            {
        //                var newUser = await RegisterExternalUserAsync(externalUser);
        //                if (!newUser.IsActive)
        //                {
        //                    return new ExternalAuthenticateResultModel
        //                    {
        //                        WaitingForActivation = true
        //                    };
        //                }

        //                // Try to login again with newly registered user!
        //                loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
        //                if (loginResult.Result != AbpLoginResultType.Success)
        //                {
        //                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
        //                        loginResult.Result,
        //                        model.ProviderKey,
        //                        GetTenancyNameOrNull()
        //                    );
        //                }

        //                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                        
        //                return new ExternalAuthenticateResultModel
        //                {
        //                    AccessToken = accessToken,
        //                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
        //                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
        //                };
        //            }
        //        default:
        //            {
        //                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
        //                    loginResult.Result,
        //                    model.ProviderKey,
        //                    GetTenancyNameOrNull()
        //                );
        //            }
        //    }
        //}

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                externalUser.Name,
                externalUser.Surname,
                externalUser.EmailAddress,
                externalUser.EmailAddress,
                Authorization.Users.User.CreateRandomPassword(),
                true
            );

            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                case AbpLoginResultType.InvalidPassword:
                    throw new UserFriendlyException("密码错误！！！");
                case AbpLoginResultType.UserIsNotActive:
                    throw new UserFriendlyException("用户未激活，请联系管理员激活");
                case AbpLoginResultType.LockedOut:
                    throw new UserFriendlyException("账号已被锁定，请联系管理员解锁");
                default:
                    throw new UserFriendlyException("未知异常，请联系管理员处理！！！!");
                    //throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

            private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity,string role)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            string key;
            string reKey;
            string reRole;
            if (role == "教师")
            {
                key = "Teacher";
                reKey = "Student";
                reRole = "学生";
            }
            else
            {
                reRole = "教师";
                reKey = "Teacher";
                key = "Student";
            }
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(key, role)
            });
            if(claims.Contains(new Claim(reKey,reRole)))
            claims.Remove(new Claim(reKey, reRole));
            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken);
        }
    }
}
