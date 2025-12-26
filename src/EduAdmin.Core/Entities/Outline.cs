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
    [Table("Outline")]
    public class Outline : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 大纲名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 教师Id
        /// </summary>
        public virtual Guid TeacherId { get; set; }
        /// <summary>
        /// 大纲类型（课程，课设）
        /// </summary>
        public virtual string Kind { get; set; }
        /// <summary>
        /// 是否完整
        /// </summary>
        public virtual bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
