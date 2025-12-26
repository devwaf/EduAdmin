using System;

namespace EduAdmin.AppService.Classess
{
    public class ClassesShowDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 学年
        /// </summary>
        public virtual string SchoolYear { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public virtual string Major { get; set; }
    }
}