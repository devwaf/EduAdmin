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
    /// <summary>
    /// 班级
    /// </summary>
    [Table("Classes")]
    public class Classes : Entity<Guid>, ICreationAudited, IModificationAudited,ISoftDelete
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 学年
        /// </summary>
        public virtual string SchoolYear { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public virtual string Major { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
