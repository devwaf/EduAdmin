using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.TeachingTasks.Dto
{
    public class AddTeachingTaskDto
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
        public TeaTaskContentAny Content { get; set; }

    }
    public class TeaTaskContentAny 
    {
        public virtual TeaTaskContent Num1 { get; set; }
        public virtual TeaTaskContent Num2 { get; set; }
        public virtual TeaTaskContent Num3 { get; set; }
        public virtual TeaTaskContent Num4 { get; set; }
        public virtual TeaTaskContent Num5 { get; set; }
        public virtual TeaTaskContent Num6 { get; set; }
        public virtual TeaTaskContent Num7 { get; set; }
        public virtual TeaTaskContent Num8 { get; set; }
        public virtual TeaTaskContent Num9 { get; set; }
        public virtual TeaTaskContent Num10 { get; set; }
        public virtual TeaTaskContent Num11 { get; set; }
        public virtual TeaTaskContent Num12 { get; set; }
        public virtual TeaTaskContent Num13 { get; set; }
        public virtual TeaTaskContent Num14 { get; set; }
        public virtual TeaTaskContent Num15 { get; set; }
        public virtual TeaTaskContent Num16 { get; set; }
    }

    public class TeaTaskContent
    {
        /// <summary>
        /// 是否合格
        /// </summary>
        public string IsPass { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
