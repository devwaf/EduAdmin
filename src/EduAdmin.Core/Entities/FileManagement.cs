using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EduAdmin.Entities
{
    /// <summary>
    /// 文件管理
    /// </summary>
    [Table("FileManagement")]
    public class FileManagement : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 文件类型（后缀）
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 来源类型Key ： 项目文件 公司文件 任务文件 个人文件
        /// </summary>
        public Guid? ProjectKey { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual string Size { get; set; }
        /// <summary>
        /// 相对路径（防止线上服务器迁移或修改域名导致出错的问题）
        /// </summary>
        public virtual string RelativePath { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
    }
}
