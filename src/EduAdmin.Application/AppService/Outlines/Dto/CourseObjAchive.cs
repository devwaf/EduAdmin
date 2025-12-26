using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Outlines.Dto
{
    public  class CourseObjAchive
    {
        /// <summary>
        /// 课程目标达成度
        /// </summary>
        public float Score { get; set; }
        /// <summary>
        /// 课程目标名称
        /// </summary>
        public string CourseObjectiveName { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public Guid StudentId { get; set; }
    }
    public class AllCompute
    {
        public List<CourseObjAchive> CourseObjAchives { get; set; }
        /// <summary>
        /// 单条详细目标达成度
        /// </summary>
        public List<Dictionary<string, string>> ComputeAlone { get; set; }
    }
}
