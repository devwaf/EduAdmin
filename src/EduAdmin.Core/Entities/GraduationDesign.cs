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
    /// 毕业设计
    /// </summary>
    [Table("GraduationDesign")]
    public class GraduationDesign : Entity<Guid>, ICreationAudited, IModificationAudited, ISoftDelete
    {
        /// <summary>
        /// 毕业设计名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 毕业设计状态
        /// </summary>
        public virtual bool? State { get; set; }
        /// <summary>
        /// 任务书
        /// </summary>
        public virtual string Assignment { get; set; }
        /// <summary>
        /// 开题报告
        /// </summary>
        public virtual string Headline { get; set; }
        /// <summary>
        /// 外文翻译
        /// </summary>
        public virtual string ForeignTrans { get; set; }
        /// <summary>
        /// 论文草稿
        /// </summary>
        public virtual string DraftDissertation { get; set; }
        /// <summary>
        /// 第一阶段情况报告
        /// </summary>
        public virtual string FirstReport  { get; set; }
        /// <summary>
        /// 第二阶段情况报告
        /// </summary>
        public virtual string SecondReport { get; set; }
        /// <summary>
        /// 论文
        /// </summary>
        public virtual string Dissertation { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public virtual string Annex { get; set; }
        /// <summary>
        /// 查重报告
        /// </summary>
        public virtual string CheckReport { get; set; }
        public bool IsDeleted { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
