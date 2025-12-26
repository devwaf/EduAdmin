using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using EduAdmin.Entities;
using EduAdmin.FileManagements.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduAdmin;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using Abp.ObjectMapping;
using Abp.Domain.Uow;

namespace EduAdmin.FileManagements
{
    /// <summary>
    /// 文件处理
    /// </summary>
    [AbpAuthorize]
    public class FileManagementAppService : EduAdminAppServiceBase, IFileManagementAppService
    {
        // 临时文件夹 Temporary (临时压缩文件等)
        // 任务文件夹 TaskFiles (执行任务过程的文件及其导出文件)
        // 文件夹 Files  (文件管理的主动上传的文件)
        // 模板文件夹 TemplateFiles  (文件管理的主动上传的文件)
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IRepository<FileManagement, Guid> _fileManagementEFRepository;
        private readonly IRepository<Outline, Guid> _outlineEFRepository;
        private readonly IRepository<ScoreWeight, Guid> _scoreWeightEFRepository;
        private readonly IObjectMapper _mapper;
        public FileManagementAppService(
            IWebHostEnvironment env,
            IConfiguration configuration,
            IRepository<FileManagement, Guid> fileManagementEFRepository,
            IRepository<Outline, Guid> outlineEFRepository,
            IRepository<ScoreWeight, Guid> scoreWeightEFRepository,
            IObjectMapper mapper
            )
        {
            _env = env;
            _configuration = configuration;
            _fileManagementEFRepository = fileManagementEFRepository;
            _outlineEFRepository = outlineEFRepository;
            _scoreWeightEFRepository = scoreWeightEFRepository;
            _mapper = mapper;
        }
        #region 文件管理

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Guid> AddFile(FileCreateDto file)
        {
            file.RelativePath = CheckPath(file.RelativePath);
            var newfile = _mapper.Map<FileManagement>(file);
            var fileId = await _fileManagementEFRepository.InsertAndGetIdAsync(newfile);
            return fileId;
        }
        /// <summary>
        /// 替换文件（目前仅用于头像）
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task ReplaceFile(Guid fileId, string path)
        {
            path = CheckPath(path);
            var file = await _fileManagementEFRepository.GetAsync(fileId); ;
            var oldpath = file.RelativePath.Replace("@", _env.WebRootPath);
            if (File.Exists(oldpath))
                File.Delete(oldpath);
            file.RelativePath = path;
            await _fileManagementEFRepository.UpdateAsync(file);
        }
        /// <summary>
        /// 删除文件(标记删除)
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFile(Guid fileId)
        {
            try
            {
                var file = await _fileManagementEFRepository.GetAsync(fileId);
                file.IsDeleted = true;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 多文件删除(标记删除)
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task DeleteListFile(List<FileDto> files)
        {
            if (files == null || files.Count == 0)
                return;
            foreach (var item in files) {
                await DeleteFile(item.Id);
            }
        }
        /// <summary>
        /// 删除文件(标记删除)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFileByPath(string path)
        {
            path = CheckPath(path);
            var file = await _fileManagementEFRepository.FirstOrDefaultAsync(c => c.RelativePath == path);
            file.IsDeleted = true;
            return true;
        }
        /// <summary>
        /// 强制删除文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<bool> ForceDeleteFile(Guid fileId)
        {
            var file = await _fileManagementEFRepository.GetAsync(fileId);
            var path = file.RelativePath.Replace("@", _env.WebRootPath);
            if (File.Exists(path))
                File.Delete(path);
            else
                return false;
            await _fileManagementEFRepository.HardDeleteAsync(file);
            return true;
        }
        /// <summary>
        /// 强制删除多个文件
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public async Task<bool> ForceDeleteFiles(List<Guid> fileIds)
        {
            foreach(var fileId in fileIds)
            {
                await ForceDeleteFile(fileId);
            }
            return true;
        }
        /// <summary>
        /// 通过ID获得文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<FileManagement> GetFileById(Guid fileId)
        {
            var file = await _fileManagementEFRepository.FirstOrDefaultAsync(fileId);
            return file;
        }

        /// <summary>
        /// 修改文件名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateFileById(UpdateFileInputDto input)
        {
            var file = await _fileManagementEFRepository.GetAsync(new Guid(input.FileId));
            file.Name = input.Name;
            return new UpdateResult();
        }
        /// <summary>
        /// 转为@的相对路径（存储使用）
        /// </summary>
        /// <param name="path"></param>
        public string CheckPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            else if (!path.Contains("@"))
            {
                path = path.Replace(_env.WebRootPath, "@");
                path = path.Replace(LocalTool.GetAppSettings("path"), "@");
            }
            return path;
        }
        /// <summary>
        /// 本地路径（程序使用）
        /// </summary>
        /// <param name="path"></param>
        public string PathToLocal(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            else if (!path.Contains(_env.WebRootPath))
            {
                path = path.Replace("@", _env.WebRootPath);
                path = path.Replace(LocalTool.GetAppSettings("path"), _env.WebRootPath);
            }
            return path;
        }
        /// <summary>
        /// 线上路径（下载用）
        /// </summary>
        /// <param name="path"></param>
        public string PathToRelative(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            else if (!path.Contains(LocalTool.GetAppSettings("path")))
            {
                path = path.Replace("@", LocalTool.GetAppSettings("path"));
                path = path.Replace(_env.WebRootPath, LocalTool.GetAppSettings("path"));
            }
            return path;
        }
        #endregion
        #region 服务导出图片检测处理
        /// <summary>
        /// 服务导出图片检测处理
        /// </summary>
        /// <param name="localImg"></param>
        /// <returns></returns>
        public string CheckFFmpegImg(string localImg)
        {
            //验证文件
            if (!File.Exists(localImg))
                return null;
            //保存文件夹
            string savePath = Path.Combine(_env.WebRootPath, "FFmpegPic");
            string localres = Path.Combine(savePath, Path.GetFileName(localImg));
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            //移动文件
            using (var stream = new FileStream(localres, FileMode.Create)) //文件的移动到指定的目录
            {
                var file = File.OpenRead(localImg);
                file.CopyTo(stream);
                stream.Close();
                file.Close();
            }
            //成功后删除原文件
            if (File.Exists(localres))
            {
                File.Delete(localImg);
                return PathToRelative(localres);
            }
            else
                return null;
        }
        #endregion
        #region 其他模块页面上传文件图片时调用

        //public async Task<UploadOutputDto> UploadAsync(string projectName, string fileType, IFormFile file)
        //{
        //    var result = new UploadOutputDto();
        //    try
        //    {
        //        if (file != null)
        //        {
        //            result.FileName = file.FileName;
        //            result.FileType = fileType;

        //            var time = DateTime.Now;
        //            var filePath = Path.Combine(_env.WebRootPath, AppConsts.DefaultFilePath, projectName, time.ToString("yyyyMMdd"));
        //            if (!Directory.Exists(filePath))
        //            {
        //                Directory.CreateDirectory(filePath);
        //            }
        //            filePath = Path.Combine(filePath, $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }
        //            result.FilePath = filePath.Replace(_env.WebRootPath, _configuration["ServicePath"]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error($"UploadAsync(文件上传)功能异常。详情：{projectName}--{file.FileName}{ ex.Message}。内部异常：{ ex.InnerException}");
        //    }
        //    return result;
        //}
        #endregion

        #region 当磁盘空间快满时，自动清理标记文件
        /// <summary>
        /// 清理两周前的文件和导出文件
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]//不需登录即可调用
        public async Task<string> DeleteInActiveFile()
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var FileList = await _fileManagementEFRepository.GetAll()
                    .Where(c => c.IsDeleted == true && c.CreationTime.AddDays(14)<DateTime.Now)
                    .ToListAsync();
                foreach (var item in FileList)
                {
                    string path = item.RelativePath.Replace("@", _env.WebRootPath);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    _fileManagementEFRepository.HardDelete(item);
                }
                var srcPath = Path.Combine(_env.WebRootPath, "ExportExcels");
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
                foreach (FileSystemInfo i in fileinfo)
                {
                    if(!(i is DirectoryInfo))
                    File.Delete(i.FullName);
                }
            }
            return "文件自动删除成功";
        }

        /// <summary>
        /// 清理标记文件
        /// </summary>
        /// <returns></returns>
        public async Task<string> DeleteHasFile()
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var FileList = await _fileManagementEFRepository.GetAll()
                    .Where(c=>c.IsDeleted == true)
                    .ToListAsync();
                foreach (var item in FileList)
                {
                    string path = item.RelativePath.Replace("@", _env.WebRootPath);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    _fileManagementEFRepository.HardDelete(item);
                }
            }
            return "文件自动删除成功";
        }
        /// <summary>
        /// 清理标记数据
        /// </summary>
        /// <returns></returns>
        public async Task<string> DeleteHasData()
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                await _outlineEFRepository.HardDeleteAsync(c => c.IsDeleted == true);
            }
            return "文件自动删除成功";
        }
        /// <summary> 
        /// 获取指定驱动器的空间总大小(单位为B) (未测试)
        /// </summary> 
        /// <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        /// <returns> </returns> 
        public static long GetHardDiskSpace(string str_HardDiskName)
        {
            long totalSize = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    totalSize = drive.TotalSize;
                }
            }
            return totalSize;
        }

        /// <summary> 
        /// 获取指定驱动器的剩余空间总大小(单位为B) (未测试)
        /// </summary> 
        /// <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        /// <returns> </returns> 
        public static long GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long freeSpace = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace;
                }
            }
            return freeSpace;
        }
        #endregion
    }
}
