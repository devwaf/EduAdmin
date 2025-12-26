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
    [Table("StudentHomework")]
    public class StudentHomework : AuditedEntity<Guid>,ISoftDelete
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public virtual Guid StudentId { get; set; }
        /// <summary>
        /// 作业Id
        /// </summary>
        public virtual Guid HomeworkId { get; set; }
        /// <summary>
        /// 评语
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual double? Score { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public virtual string FilePath { get; set; }
        /// <summary>
        /// 作业提交状态
        /// </summary>
        public virtual bool? State { get; set; }
        public bool IsDeleted { get; set; }
    }
}
