using EduAdmin.LocalTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.FileManagements.Dto
{
    public class FileOutputDto
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
        /// 文件类型（后缀）
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? CategoryKey { get; set; }
        /// <summary>
        /// 来源类型 ： 项目文件 公司文件 任务文件 个人文件
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 查询路径
        /// </summary>
        public string Dir { get; set; }
        /// <summary>
        /// 文件标记（方便查找或共享至其他平台）
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 相对路径（防止线上服务器迁移或修改域名导致出错的问题）
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 详细描述 (Json键值对 描述具体信息)
        /// </summary>
        public List<TagDto> Tags { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 创建或修改时间
        /// </summary>
        public string LastTime { get; set; }
    }
    public class FileShortOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件归属
        /// </summary>
        public string FileAscription { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 相对路径（防止线上服务器迁移或修改域名导致出错的问题）
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public long? Creator { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 创建或修改时间
        /// </summary>
        public string LastTime { get; set; }
        /// <summary>
        /// 是不是项目
        /// </summary>
        public bool IsProject { get; set; }
        /// <summary>
        /// 是不是模块
        /// </summary>
        public bool IsDir { get; set; }
    }
    public class FileDto
    {
        /// <summary>
        /// fileId
        /// </summary>
        public Guid Id;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName;
        /// <summary>
        /// 调用路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type;
        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size;
    }
}
