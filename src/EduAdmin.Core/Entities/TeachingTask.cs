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
    /// 教学任务表
    /// </summary>
    [Table("TeachingTask")]
    public class TeachingTask : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public virtual Guid ClassId { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public virtual Guid CourseId { get; set; }
        /// <summary>
        /// 对应序号的填充数据
        /// </summary>
        public virtual string Num1 { get; set; }
        public virtual string Num2 { get; set; }
        public virtual string Num3 { get; set; }
        public virtual string Num4 { get; set; }
        public virtual string Num5 { get; set; }
        public virtual string Num6 { get; set; }
        public virtual string Num7 { get; set; }
        public virtual string Num8 { get; set; }
        public virtual string Num9 { get; set; }
        public virtual string Num10 { get; set; }
        public virtual string Num11 { get; set; }
        public virtual string Num12 { get; set; }
        public virtual string Num13 { get; set; }
        public virtual string Num14 { get; set; }
        public virtual string Num15 { get; set; }
        public virtual string Num16 { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public virtual string FilePath { get; set; }
        public bool IsDeleted { get; set; }
    }
}
