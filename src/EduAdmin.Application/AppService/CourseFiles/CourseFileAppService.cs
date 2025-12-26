using Abp.Authorization;
using Abp.Domain.Repositories;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.FileManagements;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.CourseFiles
{
    [AbpAuthorize]
    public  class CourseFileAppService : EduAdminAppServiceBase, ICourseFileAppService
    {
        private readonly IRepository<CourseFile,Guid> _courseFileRepository;
        private readonly IWebHostEnvironment _env;
        public readonly IFileManagementAppService _fileManagementAppService;
        public CourseFileAppService(IRepository<CourseFile, Guid> courseFileRepository,
            IFileManagementAppService fileManagementAppService,
            IWebHostEnvironment env)
        {
            _courseFileRepository = courseFileRepository;
            _env = env;
            _fileManagementAppService = fileManagementAppService;
        }

        /// <summary>
        /// 添加或修改课设相关文件
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ResultDto> AddOrUpdateCourseFile(Guid courseId, string fileName, string url)
        {
            var courseFile = await _courseFileRepository.FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (courseFile == null)
            {
                if (fileName == "课程报告")
                {
                    await _courseFileRepository.InsertAsync(new CourseFile
                    {
                        CourseId = courseId,
                        CourseReport = url.Replace(LocalTool.GetAppSettings("Path"), "@")
                    });
                }
                else if (fileName == "任务书")
                {
                    await _courseFileRepository.InsertAsync(new CourseFile
                    {
                        CourseId = courseId,
                        TaskBook = url.Replace(LocalTool.GetAppSettings("Path"),"@")
                    });
                }
            }
            else
            {
                if (fileName == "课程报告")
                {
                    var filePath = _fileManagementAppService.PathToLocal(courseFile.CourseReport);
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    courseFile.CourseReport = url.Replace(LocalTool.GetAppSettings("Path"), "@");
                    await _courseFileRepository.UpdateAsync(courseFile);
                }
                else if (fileName == "任务书")
                {
                    var filePath = _fileManagementAppService.PathToLocal(courseFile.TaskBook);
                    if(File.Exists(filePath))
                    File.Delete(filePath);
                    courseFile.TaskBook = url.Replace(LocalTool.GetAppSettings("Path"), "@");
                    await _courseFileRepository.UpdateAsync(courseFile);
                }
            }
            return new ResultDto(true,"成功");
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<ResultDto> GetCourseFile(Guid courseId,string fileName)
        {
            var courseFile = await _courseFileRepository.FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (courseFile == null)
                return new ResultDto(false, "文件未上传，无法下载");
            if(fileName == "课程报告")
            {
                if(courseFile.CourseReport == null)
                    return new ResultDto(false, "文件未上传，无法下载");
                return new ResultDto(true, _fileManagementAppService.PathToRelative(courseFile.CourseReport));
            }else if (fileName == "任务书")
            {
                if (courseFile.TaskBook == null)
                    return new ResultDto(false, "文件未上传，无法下载");
                return new ResultDto(true, _fileManagementAppService.PathToRelative(courseFile.TaskBook));
            }
            return new ResultDto(false, "不存在此文件");
        }
        /// <summary>
        /// 预览文件
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ResultDto> RevieweCourseFile(Guid courseId, string fileName)
        {
           var res = await GetCourseFile(courseId, fileName);
            if (res.Result == false)
                return res;
            var localPath = _fileManagementAppService.PathToLocal(res.Message);
            var outPath = Path.Combine(_env.WebRootPath, "PDFs", "CourseFile");
            var pdfPath = OfficeConvert.GetFilePdf(localPath, outPath, _env.WebRootPath, LocalTool.GetAppSettings("path"));
            return new ResultDto(true, pdfPath);
        }
    }
}
