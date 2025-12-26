using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.FileManagements.Dto
{
    public class FileInputDto
    {
        /// <summary>
        /// 所处公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 项目Id 公司Id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string Dir { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public string StageName { get; set; }
        /// <summary>
        /// 文件夹Id
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UpdateFileInputDto
    {
        /// <summary>
        /// 文件夹Id
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

    }
}
