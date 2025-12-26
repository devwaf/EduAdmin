using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.CourseObjectives.Dto
{
    public class CreateCourseObjectiveDto
    {
        /// <summary>
        /// 大纲
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        /// <summary>
        /// 课程目标列表
        /// </summary>
        public virtual List<CourseObjDto> CourseObjs { get; set; }
        
    }
    public class CourseObjDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 课程目标内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 课程目标名称
        /// </summary>
        public virtual string Name { get; set; }
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
        //public virtual int? GredeProportion { get; set; }
    }
    public class ScoreProportion
    {
        /// <summary>
        /// 成绩权重名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权重占比
        /// </summary>
        public float Power { get; set; }
    }
}
