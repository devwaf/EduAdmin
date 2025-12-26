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
    /// 毕业要求的指标点
    /// </summary>
    [Table("Target")]
    public class Target : AuditedEntity<Guid>
    {
        /// <summary>
        /// 毕业要求Id
        /// </summary>
        public virtual Guid GraduationRequireId { get; set; }
        /// <summary>
        /// 指标点名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 指标内容
        /// </summary>
        public virtual string Content { get; set; }
    }
}
