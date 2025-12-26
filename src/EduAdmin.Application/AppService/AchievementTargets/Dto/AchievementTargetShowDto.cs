using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.AchievementTargets.Dto
{
    public class AchievementTargetShowDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id  { get; set; }
        /// <summary>
        /// 指标
        /// </summary>
        public virtual string Target { get; set; }
        /// <summary>
        /// 指标满分
        /// </summary>
        public virtual int Score { get; set; }
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsActive { get; set; }
    }
    public class ShowHasAchiveTargetInput
    {
        public Guid OutlineId { get; set; }
        //public List<Guid> AchiveTargetId { get; set; }
    }
}
