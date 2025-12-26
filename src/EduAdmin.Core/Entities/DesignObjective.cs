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
    [Table("DesignObjective")]
    public class DesignObjective : Entity<Guid>, ICreationAudited, IModificationAudited, ISoftDelete
    {
        /// <summary>
        /// 大纲
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        /// <summary>
        /// 课程目标名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 毕业要求Id
        /// </summary>
        public virtual Guid? GraduationRequirementId { get; set; }
        /// <summary>
        /// 支撑度选择
        /// </summary>
        public virtual string DegreeSupport { get; set; }
        /// <summary>
        /// 分数占比
        /// </summary>
        public virtual string ScoreProportion { get; set; }
        /// <summary>
        /// 成绩占比
        /// </summary>
        public virtual int? GredeProportion { get; set; }
        public bool IsDeleted { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
