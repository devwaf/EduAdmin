using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.CourseObjectives.Dto
{
    public class CourseObjectiveShowDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 课程目标名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 课程目标内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 毕业要求
        /// </summary>
        public virtual string GraduationRequirement { get; set; }
        /// <summary>
        /// 毕业要求Id
        /// </summary>
        public virtual Guid? GraduationRequirementId { get; set; }
        /// <summary>
        /// 支撑度选择
        /// </summary>
        public virtual string DegreeSupport { get; set; }
        ///// <summary>
        ///// 分数占比
        ///// </summary>
        //public List<ScoreProportion> ScoreRate { get; set; }
        ///// <summary>
        ///// 成绩占比
        ///// </summary>
        //public virtual int GredeProportion { get; set; }
    }
}
