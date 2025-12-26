using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Users.Dto
{
    public class ProhibitPermissionInput
    {
        public long UserId { get; set; }
        public string PermissionName { get; set; }
    }
    public class UpdateRolePermissionsInput
    {
        public int RoleId { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
