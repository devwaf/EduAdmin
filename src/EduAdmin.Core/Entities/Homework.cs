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
    /// 作业
    /// </summary>
    [Table("Homework")]
    public class Homework : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 班级ID
        /// </summary>
        public virtual Guid ClassesId { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual string FileType { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public virtual Guid CourseId { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual int? Score { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public virtual int Times { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public virtual DateTime? ClosingDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
