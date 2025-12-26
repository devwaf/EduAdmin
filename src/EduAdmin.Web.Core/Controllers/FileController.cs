using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Masuit.Tools.AspNetCore.ResumeFileResults.Extensions;
using Masuit.Tools.AspNetCore.ResumeFileResults.ResumeFileResult;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduAdmin.Entities;
using EduAdmin.FileManagements;
using EduAdmin.FileManagements.Dto;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Abp.Authorization;
using EduAdmin.Controllers;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;

namespace EduAdmin.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FileController : EduAdminControllerBase
    {
        // 临时文件夹 Temporary (临时压缩文件等)
        // 任务文件夹 TaskFiles (执行任务过程的文件及其导出文件)
        // 文件夹 Files  (文件管理的主动上传的文件，模板文件)
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IFileManagementAppService _fileManagementAppService;
        

        public FileController(IHostingEnvironment env,
            IConfiguration configuration,
            IFileManagementAppService fileManagementAppService)
        {
            _env = env;
            _configuration = configuration;
            _fileManagementAppService = fileManagementAppService;
        }
        #region 普通的单个文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Module"></param>
        /// <param name="ProjectKey"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<FileDto> FileUpload(IFormFile File,
            [FromForm] string Module, [FromForm] Guid? ProjectKey, 
            [FromForm] string Description)
        {
            //首先将文件保存到指定路径
            //var file1 = Request;
            //var file2 = Request.Form;
            //var file3 = Request.Form.Files;
            //var file = Request.Form.Files.First(); //获取文件
            //var fileName = Request.Form["fileName"];//获取参数
            var file = File;
            //文件有中文，手机端显示不出来
            //string fileName = GetTrueFileName(Path.GetFileNameWithoutExtension(file.FileName))+ LocalTool.GetTimeStamp(DateTime.Now) + Path.GetExtension(file.FileName);
            // 检验文件名是否合法
            //if (!RegexLib.IsValidFileName(file.Name))
            //    throw new Exception("文件名不合法");
            string fileName = LocalTool.GetTimeStamp(DateTime.Now) + Path.GetExtension(file.FileName);

            string path = Path.Combine(_env.WebRootPath,"Files",Module);
            string savePath = Path.Combine(path,fileName);//本地存储路径
            string filePath = Path.Combine(LocalTool.GetAppSettings("path"),"Files",Module,fileName);//文件访问路径
            var fileDto = new FileCreateDto()
            {
                Type = Path.GetExtension(file.FileName),
                Module = Module,
                ProjectKey = ProjectKey,
                Description = Description,
                Size = LocalTool.GetFileSize(file.Length),
                Name = file.FileName,
                RelativePath = savePath.Replace(_env.WebRootPath, "@"),
                IsDeleted = false
            };
            if (file != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                // 备份小于100M的文件
                if (file.Length <= 100 * 1024 * 1024)
                {
                    string backupPath = LocalTool.GetAppSettings("BackupPath");
                    if (!string.IsNullOrEmpty(backupPath))
                    {
                        string backupSavePath = Path.Combine(backupPath, fileName);//本地存储路径
                        if (Directory.Exists(backupPath))
                        {
                            using (var stream = new FileStream(backupSavePath, FileMode.Create)) //文件的移动到指定的目录
                            {
                                file.CopyTo(stream);
                                stream.Close();
                            }
                        }
                    }
                }
                
                using (var stream = new FileStream(savePath, FileMode.Create)) //文件的移动到指定的目录
                {
                    file.CopyTo(stream);
                    stream.Close();
                }
                var extension = Path.GetExtension(file.FileName.ToLower());
                FileDto res = new FileDto()
                {
                    Id = await _fileManagementAppService.AddFile(fileDto),
                    Path = filePath,
                    FileName = file.FileName,
                    Type =
                       extension.Equals(".jpg")
                    || extension.Equals(".jpeg")
                    || extension.Equals(".png")
                    || extension.Equals(".gif")
                    || extension.Equals(".tiff")
                        ? "image"
                        : extension.Substring(1),
                    Size = LocalTool.GetFileSize(file.Length)
                };
                return res;
            }
           
            throw new Exception("没有得到文件！");
        }
        /// <summary>
        /// 去除文件非法字符名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetTrueFileName(string name)
        {
            name.Replace(" ", "");
            name.Replace("/", "");
            name.Replace(@"\", "");
            name.Replace(@":", "");
            name.Replace(@"*", "");
            name.Replace("\"", "");
            name.Replace(@"?", "");
            name.Replace(@"<", "");
            name.Replace(@">", "");
            name.Replace(@"|", "");
            return name + "_";
        }
        #region 文件下载
        /// <summary>
        /// 通过文件路径提供下载，支持续传
        /// </summary>
        /// <param name="filePath">指定的文件</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DownloadFiles(string filePath)
        {
            //参考JSL代码
            var localpath = ToLocalPath(filePath);
            FileStream stream = System.IO.File.OpenRead(filePath);
            ResumeFileStreamResult result = this.ResumeFile(stream, "application/octet-stream", localpath);
            return result;
        }
        /// <summary>
        /// 路径替换为本地路径
        /// </summary>
        /// <param name="path"></param>
        private string ToLocalPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("文件不能为空！");
            else
            {
                path = path.Replace("@", _env.WebRootPath);
                path = path.Replace(LocalTool.GetAppSettings("path"), _env.WebRootPath);
            }
            return path;
        }
        #endregion
        
        #region Excel导入
        /// <summary>
        /// Excel导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AbpMvcAuthorize]
        public async Task GetTopicFromExcel(IFormFile File, [FromForm] string Module, [FromForm] Guid? ProjectKey, [FromForm] string Description)
        {
            //首先将文件保存到指定路径
            //var file = Request.Form.Files.First(); //获取文件
            var file = File;
            string fileName = LocalTool.GetTimeStamp(DateTime.Now) + Path.GetExtension(file.FileName);
            string path = Path.Combine(_env.WebRootPath, "Excel");
            string savePath = Path.Combine(path, fileName);//本地存储路径
            //string filePath =LocalTool.GetAppSettings("path") + @"/Excel/" + fileName;//文件访问路径
            var fileDto = new FileCreateDto()
            {
                Type = Path.GetExtension(file.FileName),
                Module = Module,
                ProjectKey = ProjectKey,
                Description = Description,
                Size = LocalTool.GetFileSize(file.Length),
                Name = fileName,
                RelativePath = savePath.Replace(_env.WebRootPath, "@")
            };
            await _fileManagementAppService.AddFile(fileDto);
            if (file != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var stream = new FileStream(savePath, FileMode.Create)) //文件的移动到指定的目录
                {
                    file.CopyTo(stream);
                    stream.Close();
                }
            }
            //string savePath = @"D:\桌面\选题文件.xls";
            //然后将Excel内容导入到数据库中
            FileStream files = null;
            IWorkbook Workbook = null;
            try
            {
                using (files = new FileStream(savePath, FileMode.Open, FileAccess.Read))//C#文件流读取文件
                {
                    try
                    {
                        Workbook = new XSSFWorkbook(files);//XSSFWorkbook:是操作Excel2007的版本，扩展名是.xlsx
                    }
                    catch
                    {
                        Workbook = new HSSFWorkbook(files);//HSSFWorkbook:是操作Excel2003以前（包括2003）的版本，扩展名是.xls
                    }
                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0);//读取第一个sheet
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        //得到Excel工作表的行 
                        IRow headerRow = sheet.GetRow(1);
                        //得到Excel工作表的总列数  
                        int cellCount = headerRow.LastCellNum;
                        //ModelName = Path.GetFileNameWithoutExtension(file.FileName);
                        int k = 0;
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            //将指定行存入指定字段    若导入文件不规范，则会导入错乱
                            //if (row.GetCell(0) != null)
                            //    topic.Name = row.GetCell(0).ToString();
                            //if (row.GetCell(1) != null)
                            //    topic.Remark = row.GetCell(1).ToString();
                            k++;
                            if (k >= 5000)
                            {
                                await base.CurrentUnitOfWork.SaveChangesAsync();
                                k = 0;
                            }
                            //await _topicLibraryEFRepository.InsertAsync(topic);
                        }
                        await base.CurrentUnitOfWork.SaveChangesAsync();
                        files.Close();//关闭当前流并释放资源
                    }
                }
            }
            catch (Exception e)
            {
                if (files != null)
                {
                    files.Close();//关闭当前流并释放资源
                }
                throw e;
            }
        }
        #endregion
        /// <summary>
        /// 切割文件测试
        /// </summary>
        /// <param name="filePath">指定的文件</param>
        /// <param name="fileSize">单次的大小</param>
        /// <param name="index">文件的序号（第几份文件）</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TestCuts(string filePath, int fileSize, int index = 0)
        {
            filePath = @"D:\Desktop\Test\林俊杰美人鱼.mp3";
            byte[] bytsize = new byte[1024 * 1024];//1M
            if(!Directory.Exists(@"D:\Desktop\Test\Cuts"))
                Directory.CreateDirectory(@"D:\Desktop\Test\Cuts");
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                int i = 1;
                while (true)
                {
                    int r = stream.Read(bytsize, 0, bytsize.Length);
                    //如果读取到的字节数为0，说明已到达文件结尾，则退出while循
                    if (r < 1)
                    {
                        break;
                    }
                    //创建写入流
                    string newFilePath = @"D:\Desktop\Test\Cuts\林俊杰美人鱼temp" + i;
                    using (FileStream fswrite = new FileStream(newFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fswrite.Write(bytsize, 0, r);
                    }
                    i++;
                }
            }
            return Ok("");
        }
        /// <summary>
        /// 合并文件测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Testmerge()
        {
            string size = "";
            //写入流
            using (FileStream stream = new FileStream(@"D:\Desktop\Test\金龙鱼.mp3", FileMode.OpenOrCreate, FileAccess.Write))
            {
                int i = 1;
                //循环读取
                string cutPath = @"D:\Desktop\Test\Cuts\林俊杰美人鱼temp" + i;
                while (System.IO.File.Exists(cutPath))
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(cutPath);
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                    System.IO.File.Delete(cutPath);
                    i++;
                    cutPath = @"D:\Desktop\Test\Cuts\林俊杰美人鱼temp" + i;
                }
                size = LocalTool.GetFileSize(stream.Length);
            }
            Directory.Delete(@"D:\Desktop\Test\Cuts");//删除文件夹
            return Ok(size);
        }
        #endregion
    }
}
