using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreWeights.Dto
{
    public class CreateScoreWeightDto
    {
        /// <summary>
        /// 大纲Id
        /// </summary>
        public virtual Guid OutlineId { get; set; }
    }
    public class UpdateScoreWeight
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
        /// 权重名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 权重占比
        /// </summary>
        public virtual float? Power { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public virtual int? Times { get; set; }
    }
}
