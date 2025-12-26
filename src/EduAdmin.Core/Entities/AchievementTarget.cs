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
    /// 课设成绩指标
    /// </summary>
    [Table("AchievementTarget")]
    public class AchievementTarget : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 课设的评审项目Id
        /// </summary>
        public virtual Guid ScoreAchievementId { get; set; }
        /// <summary>
        /// 大纲ID
        /// </summary>
        public virtual Guid OutlineId { get; set;}
        /// <summary>
        /// 指标
        /// </summary>
        public virtual string Target { get; set; }
        /// <summary>
        /// 指标满分
        /// </summary>
        public virtual int? Score { get; set; }

        public bool IsDeleted { get; set; }
    }
}
