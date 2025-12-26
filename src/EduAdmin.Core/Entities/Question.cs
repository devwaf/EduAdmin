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
    [Table("Question")]
    public class Question : AuditedEntity<Guid>
    {
        /// <summary>
        /// 大题Id
        /// </summary>
        public virtual Guid TestQuestionId { get; set; }
        /// <summary>
        /// 大纲Id
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        /// <summary>
        /// 题号
        /// </summary>
        public virtual string TitleNum { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual int? Score { get; set; }
        /// <summary>
        /// 课程目标
        /// </summary>
        public virtual Guid? CourseObjectiveId { get; set; }
    }
}
