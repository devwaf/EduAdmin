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
    /// 学生
    /// </summary>
    [Table("Student")]
    public class Student : AuditedEntity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public virtual string Sno { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool Gender { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public Guid ClassId { get; set; }
        /// <summary>
        /// 毕业设计Id
        /// </summary>
        public virtual Guid? GraduationDesignId { get; set; }
        /// <summary>
        /// 毕业设计老师Id
        /// </summary>
        public Guid? GraDesTeacherId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
