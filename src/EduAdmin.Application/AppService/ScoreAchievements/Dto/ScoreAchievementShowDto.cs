using EduAdmin.AppService.AchievementTargets.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreAchievements.Dto
{
    public class ScoreAchievementShowDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 评审名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 评审比重
        /// </summary>
        public virtual float? Weight { get; set; }
        /// <summary>
        /// 指标
        /// </summary>
        public List<AchievementTargetShowDto> AchieveTarget { get; set; }
    }
}
