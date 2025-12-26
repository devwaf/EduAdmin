using Abp.Authorization;
using EduAdmin.Authorization.Roles;
using EduAdmin.Authorization.Users;

namespace EduAdmin.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
