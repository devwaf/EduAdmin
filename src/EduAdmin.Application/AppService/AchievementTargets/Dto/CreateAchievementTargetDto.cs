using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.AchievementTargets.Dto
{
    public class CreateAchievementTargetDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 课设的评审项目Id
        /// </summary>
        public virtual Guid ScoreAchievementId { get; set; }
        /// <summary>
        /// 大纲Id
        /// </summary>
        public Guid outlineId { get; set; }
        /// <summary>
        /// 指标
        /// </summary>
        public virtual string Target { get; set; }
        /// <summary>
        /// 指标满分
        /// </summary>
        public virtual int? Score { get; set; }
        /// <summary>
        /// 占比
        /// </summary>
        public virtual float? rate { get; set; }
    }
}
