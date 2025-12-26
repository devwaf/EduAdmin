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
    [Table("StudentCourse")]
    public class StudentCourse : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public virtual Guid StudentId { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public virtual Guid CourseId { get; set;}
        /// <summary>
        /// 学生当门课的分数
        /// </summary>
        public virtual int Score { get; set;}
        public bool IsDeleted { get; set; }
    }
}
