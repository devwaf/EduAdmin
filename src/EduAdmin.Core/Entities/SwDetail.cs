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
    [Table("SwDetail")]
    public class SwDetail : AuditedEntity<Guid>
    {
        /// <summary>
        /// 大纲Id
        /// </summary>
        public Guid OutlineId { get; set; }
        /// <summary>
        /// 权重Id
        /// </summary>
        public Guid ScoreWeightId  { get; set; }
        /// <summary>
        /// 作业代号
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 作业权重
        /// </summary>
        public float? Power { get; set; }
        /// <summary>
        /// 作业次数
        /// </summary>
        public int Times { get; set; }
        /// <summary>
        /// 课程目标Id
        /// </summary>
        public Guid? CourseObjectiveId { get; set; }
    }
}
