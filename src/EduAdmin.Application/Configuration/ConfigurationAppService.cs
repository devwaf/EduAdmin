using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using EduAdmin.Configuration.Dto;

namespace EduAdmin.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : EduAdminAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
