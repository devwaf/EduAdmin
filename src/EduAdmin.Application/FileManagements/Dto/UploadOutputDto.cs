using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.FileManagements.Dto
{
    public class UploadOutputDto
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
