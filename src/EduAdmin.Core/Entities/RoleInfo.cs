using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Entities
{
    [Table("Role")]
    public class RoleInfo : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { set; get; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public virtual string RoleType { set; get; }
        public bool IsDeleted { get; set; }
    }
}
