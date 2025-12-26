using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Users.Dto
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 人名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 班级Id
        /// </summary>
        public Guid ClassId { get; set; }
        /// <summary>
        /// 临时密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
    }
}
