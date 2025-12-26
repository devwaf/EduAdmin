using EduAdmin.Entities;
using EduAdmin.FileManagements.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.FileManagements
{
    public interface IFileManagementAppService
    {
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<Guid> AddFile(FileCreateDto file);
        /// <summary>
        /// 文件删除(标记删除)
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<bool> DeleteFile(Guid fileId);
        /// <summary>
        /// 多文件删除(标记删除)
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task DeleteListFile(List<FileDto> files);
        /// <summary>
        /// 文件删除(标记删除)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<bool> DeleteFileByPath(string path);
        /// <summary>
        /// 文件强制删除
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<bool> ForceDeleteFile(Guid fileId);
        /// <summary>
        /// 替换文件（目前仅用于头像）
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="path"></param>
        Task ReplaceFile(Guid fileId, string path);
        /// <summary>
        /// 本地路径（程序使用）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string PathToLocal(string path);
        /// <summary>
        /// 线上路径（下载用）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string PathToRelative(string path);
        /// <summary>
        /// 存储使用(@)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string CheckPath(string path);
        /// <summary>
        /// 服务导出图片检测处理
        /// </summary>
        /// <param name="localImg"></param>
        /// <returns></returns>
        string CheckFFmpegImg(string localImg);
        /// <summary>
        /// 清理删除的文件
        /// </summary>
        /// <returns></returns>
        Task<string> DeleteInActiveFile();
    }
}
