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
    /// 成绩指标
    /// </summary>
    [Table("ScoreAchievement")]
    public class ScoreAchievement : AuditedEntity<Guid>
    {
        /// <summary>
        /// 大纲Id
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 指标比重
        /// </summary>
        public virtual float? Weight { get; set; }
    }
}
