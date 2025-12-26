using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreAchievements.Dto
{
    public class CreateScoreAchievementDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 大纲Id
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 指标比重
        /// </summary>
        public virtual int? Weight { get; set; }
    }
}
