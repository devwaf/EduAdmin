using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Courses.Dto
{
    public class CreateCourseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 班级Id
        /// </summary>
        public List<Guid> ClassIds { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 教师Id
        /// </summary>
        public virtual Guid TeacherId { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public virtual string Supervisor { get; set; }
        /// <summary>
        /// 大纲Id
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        /// <summary>
        /// 课程类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 学期
        /// </summary>
        public virtual string Semester { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        public virtual int Credit { get; set; }
        /// <summary>
        /// 学时
        /// </summary>
        public virtual int ClassDuration { get; set; }
        /// <summary>
        /// 系别
        /// </summary>
        public virtual string Department { get; set; }
        /// <summary>
        /// 实验学时
        /// </summary>
        public virtual int TextDuration { get; set; }
        /// <summary>
        /// 类别（课程，课设）
        /// </summary>
        public virtual string Kind { get; set; }
    }
}
