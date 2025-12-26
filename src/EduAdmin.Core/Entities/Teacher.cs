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
    /// <summary>
    /// 教师
    /// </summary>
    [Table("Teacher")]
    public class Teacher : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public virtual string JobNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}
