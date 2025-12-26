using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.FileManagements.Dto
{
    /// <summary>
    /// 切片文件添加
    /// </summary>
    public class CutFileCreateDto
    {
        /// <summary>
        /// ID 或 MD5
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// 相对路径集合（防止线上服务器迁移或修改域名导致出错的问题）
        /// </summary>
        public virtual string RelativePath { get; set; }
    }
}
