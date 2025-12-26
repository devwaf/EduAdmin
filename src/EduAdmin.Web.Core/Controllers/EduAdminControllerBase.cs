using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace EduAdmin.Controllers
{
    public abstract class EduAdminControllerBase: AbpController
    {
        protected EduAdminControllerBase()
        {
            LocalizationSourceName = EduAdminConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
