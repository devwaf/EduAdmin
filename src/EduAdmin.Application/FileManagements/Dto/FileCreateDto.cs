using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.FileManagements.Dto
{
    /// <summary>
    /// 文件创建对象
    /// </summary>
    public class FileCreateDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 文件类型（后缀）
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 来源类型Key ： 项目文件 公司文件 任务文件 个人文件
        /// </summary>
        public Guid? ProjectKey { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 相对路径（防止线上服务器迁移或修改域名导致出错的问题）
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool IsDeleted { get; set; }
    }
    /// <summary>
    /// 文件完善对象
    /// </summary>
    public class FilePerfectDto
    {
        /// <summary>
        /// fileId
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public string StageName { get; set; }
        /// <summary>
        /// 查询路径
        /// </summary>
        public string Dir { get; set; }
        /// <summary>
        /// 来源类型 ： 项目文件 公司文件 任务文件 个人文件
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 来源类型Key ： 项目文件 公司文件 任务文件 个人文件
        /// </summary>
        public Guid? CategoryKey { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string DetailTags { get; set; }
    }
    public class TagDto
    {
        /// <summary>
        /// 类型： 项目 公司 任务 阶段
        /// </summary>
        public string Type;
        /// <summary>
        /// 类型对应名：项目名 公司名
        /// </summary>
        public string Label;
        /// <summary>
        /// 类型对应Id：项目Id 公司Id
        /// </summary>
        public string Value;
        public TagDto(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
        public TagDto(string type, string label, string value)
        {
            this.Type = type;
            this.Label = label;
            this.Value = value;
        }
    }
}
