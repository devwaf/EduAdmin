using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Entities
{
    [Table("ScoreWeight")]
    public class ScoreWeight : AuditedEntity<Guid>, ISoftDelete
    {
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
        /// <summary>
        /// 大纲Id
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
