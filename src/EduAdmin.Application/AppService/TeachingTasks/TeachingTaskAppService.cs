using Abp.Domain.Repositories;
using EduAdmin.AppService.TeachingTasks.Dto;
using EduAdmin.Entities;
using EduAdmin.FileManagements;
using EduAdmin.FileManagements.Dto;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.TeachingTasks
{
    /// <summary>
    /// 教学任务
    /// </summary>
    public class TeachingTaskAppService:EduAdminAppServiceBase, ITeachingTaskAppService
    {
        private readonly IRepository<TeachingTask, Guid> _teachingTaskEFRepository;
        private readonly IRepository<Course, Guid> _courseEFRepository;
        private readonly IRepository<Classes, Guid> _classesEFRepository;
        private readonly IRepository<Teacher, Guid> _teacherEFRepository;
        private readonly IFileManagementAppService _fileManagementAppService;
        private readonly IRepository<FileManagement, Guid> _fileManagementEFRepository;
        private readonly IHostingEnvironment _env;
        /// <summary>
        /// 控制器
        /// </summary>
        /// <param name="teachingTaskEFRepository"></param>
        /// <param name="courseEFRepository"></param>
        /// <param name="classesEFRepository"></param>
        /// <param name="env"></param>
        public TeachingTaskAppService(IRepository<TeachingTask, Guid> teachingTaskEFRepository,
            IRepository<Course, Guid> courseEFRepository,
            IRepository<Classes, Guid> classesEFRepository,
            IRepository<Teacher, Guid> teacherEFRepository,
            IFileManagementAppService fileManagementAppService,
            IRepository<FileManagement, Guid> fileManagementEFRepository,
            IHostingEnvironment env)
        {
            _teachingTaskEFRepository = teachingTaskEFRepository;
            _courseEFRepository = courseEFRepository;
            _classesEFRepository = classesEFRepository;
            _teacherEFRepository = teacherEFRepository;
            _fileManagementAppService = fileManagementAppService;
            _fileManagementEFRepository = fileManagementEFRepository;
            _env = env;
        }
        /// <summary>
        /// 添加或修改教学任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> AddOrUpdateTeachingTask(AddTeachingTaskDto input)
        {
            var teaTask = await _teachingTaskEFRepository.FirstOrDefaultAsync(c => c.ClassId == input.ClassId && c.CourseId == input.CourseId);
            if (teaTask != null)//如果存在则修改
            {
                var newTeaTask =  ObjectMapper.Map<TeachingTask>(input);
                teaTask.ClassId = newTeaTask.ClassId;
                teaTask.CourseId = newTeaTask.CourseId;
                teaTask.FilePath = newTeaTask.FilePath;
                teaTask.Num1 = newTeaTask.Num1;
                teaTask.Num10 = newTeaTask.Num10;
                teaTask.Num11 = newTeaTask.Num11;
                teaTask.Num12 = newTeaTask.Num12;
                teaTask.Num13 = newTeaTask.Num13;
                teaTask.Num14 = newTeaTask.Num14;
                teaTask.Num15 = newTeaTask.Num15;
                teaTask.Num16 = newTeaTask.Num16;
                teaTask.Num2 = newTeaTask.Num2;
                teaTask.Num3 = newTeaTask.Num3;
                teaTask.Num4 = newTeaTask.Num4;
                teaTask.Num5 = newTeaTask.Num5;
                teaTask.Num6 = newTeaTask.Num6;
                teaTask.Num7 = newTeaTask.Num7;
                teaTask.Num8 = newTeaTask.Num8;
                teaTask.Num9 = newTeaTask.Num9;
                var task = await _teachingTaskEFRepository.UpdateAsync(teaTask);
                return task.Id;
            }
            var teaTaskAd = ObjectMapper.Map<TeachingTask>(input);
            var addTask = await _teachingTaskEFRepository.InsertAndGetIdAsync(teaTaskAd);
            return addTask;
        }
        /// <summary>
        /// 获取教学任务表的内容
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<TeachingTaskShowDto> GetTeachingTaskShow(Guid classId,Guid courseId)
        {
          var teaTask = await _teachingTaskEFRepository.FirstOrDefaultAsync(c=>c.ClassId == classId && c.CourseId == courseId);
          var show =  ObjectMapper.Map<TeachingTaskShowDto>(teaTask);
          return show;
        }
        /// <summary>
        /// 导出老师任务
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<string> ExportTeaTask(Guid classId, Guid courseId)
        {
            var task = await _teachingTaskEFRepository.FirstOrDefaultAsync(c=>c.ClassId == classId && courseId == c.CourseId);
            var course = await _courseEFRepository.GetAsync(courseId);
            var teacherName = (await _teacherEFRepository.GetAsync(course.TeacherId)).Name;
            var cla = await _classesEFRepository.GetAsync(classId);
            var courseName = course.Name;
            var fileName = LocalTool.GetTimeStamp(DateTime.Now);
            string tempFilePath = Path.Combine(_env.WebRootPath, "FileTemplate/教学任务确认表.docx");
            string directory = Path.Combine(_env.WebRootPath, "Files/TeachingTask");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string outFilePath = "wwwroot/Files/TeachingTask/" + fileName + ".docx";
            var aa = JsonConvert.DeserializeObject<TeaTaskContent>(task.Num1);
            var show = ObjectMapper.Map<TeachingTaskShowDto>(task);
            Dictionary<string,string> dicts = new Dictionary<string,string>();
            dicts.Add("teacherName", teacherName);
            dicts.Add("className", cla.SchoolYear + cla.Major +cla.Name);
            dicts.Add("courseName", courseName);
            dicts.Add("Numa1", show.NumS1.IsPass);
            dicts.Add("Numb1", show.NumS1.Remark);
            dicts.Add("Numa2", show.NumS2.IsPass);
            dicts.Add("Numb2", show.NumS2.Remark);
            dicts.Add("Numa3", show.NumS3.IsPass);
            dicts.Add("Numb3", show.NumS3.Remark);
            dicts.Add("Numa4", show.NumS4.IsPass);
            dicts.Add("Numb4", show.NumS4.Remark);
            dicts.Add("Numa5", show.NumS5.IsPass);
            dicts.Add("Numb5", show.NumS5.Remark);
            dicts.Add("Numa6", show.NumS6.IsPass);
            dicts.Add("Numb6", show.NumS6.Remark);
            dicts.Add("Numa7", show.NumS7.IsPass);
            dicts.Add("Numb7", show.NumS7.Remark);
            dicts.Add("Numa8", show.NumS8.IsPass);
            dicts.Add("Numb8", show.NumS8.Remark);
            dicts.Add("Numa9", show.NumS9.IsPass);
            dicts.Add("Numb9", show.NumS9.Remark);
            dicts.Add("Numax0", show.NumS10.IsPass);
            dicts.Add("Numbx0", show.NumS10.Remark);
            dicts.Add("Numax1", show.NumS11.IsPass);
            dicts.Add("Numbx1", show.NumS11.Remark);
            dicts.Add("Numax2", show.NumS12.IsPass);
            dicts.Add("Numbx2", show.NumS12.Remark);
            dicts.Add("Numax3", show.NumS13.IsPass);
            dicts.Add("Numbx3", show.NumS13.Remark);
            dicts.Add("Numax4", show.NumS14.IsPass);
            dicts.Add("Numbx4", show.NumS14.Remark);
            dicts.Add("Numax5", show.NumS15.IsPass);
            dicts.Add("Numbx5", show.NumS15.Remark);
            dicts.Add("Numax6", show.NumS16.IsPass);
            dicts.Add("Numbx6", show.NumS16.Remark);
            WordHelp.Export(tempFilePath, outFilePath, dicts);
            FileInfo fileInfo = new FileInfo(outFilePath);
            var oldFile = await _fileManagementEFRepository.FirstOrDefaultAsync(c => c.RelativePath == task.FilePath);
            if (oldFile != null)
            await _fileManagementAppService.ForceDeleteFile(oldFile.Id);
            task.FilePath = outFilePath.Replace("wwwroot", "@");
            await _teachingTaskEFRepository.UpdateAsync(task);
            var id = await _fileManagementAppService.AddFile(new FileCreateDto
            {
                Name = fileInfo.Name,
                RelativePath = outFilePath.Replace("wwwroot", "@"),
                Size = fileInfo.Length.ToString(),
                Type = fileInfo.Extension
            });
            outFilePath = outFilePath.Replace("wwwroot", "@");
            var pdfPath = Path.Combine(_env.WebRootPath, "PDFs", "教学确认表导出");
            if (!Directory.Exists(pdfPath))
            {
                Directory.CreateDirectory(pdfPath);
            }
            var pdf = OfficeConvert.GetFilePdf(outFilePath, pdfPath, _env.WebRootPath, LocalTool.GetAppSettings("path"));
            return _fileManagementAppService.PathToRelative(outFilePath);
        }
    }
}
