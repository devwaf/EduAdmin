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
    /// 大题
    /// </summary>
    [Table("TestQuestion")]
    public class TestQuestion : AuditedEntity<Guid>
    {
        /// <summary>
        /// 大纲Id
        /// </summary>
        public virtual Guid OutlineId { get; set; }
        /// <summary>
        /// 题号
        /// </summary>
        public virtual string TitleNum { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual int? Score { get; set; }
    }
}
