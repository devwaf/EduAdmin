using EduAdmin.AppService.ScoreAchievements.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.DesignObjectives.Dto
{
    public class DesignObjectiveShowDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 课设目标名称
        /// </summary>
        public virtual string Name { get; set; }
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
        /// <summary>
        /// 分数占比
        /// </summary>
        public List<ScoreAchievementShowDto> ScoreRate { get; set; }
        /// <summary>
        /// 成绩占比
        /// </summary>
        public virtual int? GredeProportion { get; set; }
    }
}
