using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using EduAdmin.AppService.Courses.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService
{

    /// <summary>
    /// 课程设计答辩记录表
    /// </summary>
    // [AbpAuthorize]
    // [AbpAuthorize(PermissionNames.Pages_Users)]
    public class DefenseRecordAppService : EduAdminAppServiceBase, IDefenseRecordAppService
    {
        private readonly IRepository<DesignDefenseRecord, Guid> _designDefenseRecord;
        private readonly IRepository<Classes, Guid> _classesEFRepository;
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IHostingEnvironment _env;
        public DefenseRecordAppService(IRepository<DesignDefenseRecord, Guid> designDefenseRecord, IHostingEnvironment env,
            IRepository<Classes, Guid> classesEFRepository,
            IRepository<Student, Guid> studentEFRepository)
        {
            _designDefenseRecord = designDefenseRecord;
            _classesEFRepository = classesEFRepository;
            _studentEFRepository = studentEFRepository;
            _env = env;
        }

        /// <summary>
        /// 添加答辩记录表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<ResultDto> AddDesignDefenseObjective(CreateDefenseRecordDto input)
        {
            var defenseRecord = await _designDefenseRecord.FirstOrDefaultAsync(c=>c.StudentId == input.StudentId && c.CourseDesignId == input.CourseDesignId);
            var model = ObjectMapper.Map<DesignDefenseRecord>(input);
            model.State = 1;
            model.CreationTime = DateTime.Now;
            //  model.StudentId = Guid.NewGuid();//获取学生ID
            model.Members = JsonConvert.SerializeObject(input.Members);
            model.WordUrl = await CreateWord(model, "add");
            if (defenseRecord == null)
            {
                var id = await _designDefenseRecord.InsertAndGetIdAsync(model);
                return new ResultDto(true,"保存成功");
            }
            else
            {
                if (defenseRecord.State == 2)
                    return new ResultDto(false, "无法修改");
                defenseRecord.AcademicAdvisor = model.AcademicAdvisor;
                defenseRecord.AnswerOne = model.AnswerOne;
                defenseRecord.AnswerThree = model.AnswerThree;
                defenseRecord.AnswerTwo = model.AnswerTwo;
                defenseRecord.ClassName = model.ClassName;
                defenseRecord.CollegeName = model.CollegeName;
                defenseRecord.DefenseOpinion = model.DefenseOpinion;
                defenseRecord.DefenseScore = model.DefenseScore;
                defenseRecord.DefenseTime = model.DefenseTime;
                defenseRecord.GroupLeader = model.GroupLeader;
                defenseRecord.State = model.State;
                defenseRecord.Sno = model.Sno;
                defenseRecord.Subject = model.Subject;
                defenseRecord.Major = model.Major;
                defenseRecord.Members = model.Members;
                defenseRecord.JobDescription = model.JobDescription;
                defenseRecord.WordUrl = model.WordUrl;
                defenseRecord.QuestionOne= model.QuestionOne;
                defenseRecord.QuestionThree= model.QuestionThree;
                defenseRecord.QuestionTwo= model.QuestionTwo;
                defenseRecord.StudentName = model.StudentName;
                defenseRecord.CreationTime = model.CreationTime;
                await _designDefenseRecord.UpdateAsync(defenseRecord);
                return new ResultDto(true, "修改成功");
            }
        }
        /// <summary>
        /// 老师审核答辩记录表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<UpdateResult> UpdateDesignDefenseObjective(UpdateDefenseRecordDto input)
        {
            var model = _designDefenseRecord.Get(input.Id);

            //  var model = ObjectMapper.Map<DesignDefenseRecord>(input);
            model.DefenseScore = input.DefenseScore;
            model.DefenseOpinion = input.DefenseOpinion;
            //model.GroupLeader = input.GroupLeader;
            model.State = 2;
            model.DefenseTime = DateTime.Now;
            model.WordUrl = await CreateWord(model, "update");
            await _designDefenseRecord.UpdateAsync(model);
            return new UpdateResult();
        }

        /// <summary>
        /// 删除答辩记录表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<DeleteResult> DeleteDesignDefenseObjective(Guid id)
        {
            await _designDefenseRecord.DeleteAsync(id);
            return new DeleteResult();
        }

        /// <summary>
        /// 获取答辩记录表
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseDesignId"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<DefenseRecordShowDto> GetDesignDefense(Guid studentId,Guid courseDesignId)
        {
            var model = await _designDefenseRecord.FirstOrDefaultAsync(c=>c.StudentId == studentId && c.CourseDesignId == courseDesignId);
            //var outputPath = model.WordUrl.Replace(ConfigurationManager.AppSettings["Path"], _env.WebRootPath);
            //// pdf的文件夹地址
            //var pdfPath = Path.Combine(_env.WebRootPath, "PDFs", "DesignDefense");
            //if (!Directory.Exists(pdfPath))
            //{
            //    Directory.CreateDirectory(pdfPath);
            //}
            //var PdfUrl = OfficeConvert.GetFilePdf(outputPath, pdfPath, _env.WebRootPath, LocalTool.GetAppSettings("path"));
            if (model == null)
            {
                return null;
            }
            var result = ObjectMapper.Map<DefenseRecordShowDto>(model);
            //result.PdfUrl = PdfUrl;
            return result;
        }
        /// <summary>
        /// 获取答辩记录表文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<DesignDefenseTeaShowDto> GetDesignDefenseFile(Guid id)
        {
            var model = await _designDefenseRecord.GetAsync(id);
            var outputPath = model.WordUrl.Replace(ConfigurationManager.AppSettings["Path"], _env.WebRootPath);
            // pdf的文件夹地址
            var pdfPath = Path.Combine(_env.WebRootPath, "PDFs", "DesignDefense");
            if (!Directory.Exists(pdfPath))
            {
                Directory.CreateDirectory(pdfPath);
            }
            var PdfUrl = OfficeConvert.GetFilePdf(outputPath, pdfPath, _env.WebRootPath, LocalTool.GetAppSettings("path"));
            return new DesignDefenseTeaShowDto
            {
                DefenseScore = model.DefenseScore,
                DefenseOption = model.DefenseOpinion,
                PdfUrl = PdfUrl,
            };
        }
        /// <summary>
        /// 获取一个班级学生答辩记录表的批改状态
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="courseDesignId"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<PageOutput<DefenseRecordForClassAndStu>> GetDesignDefenseList(Guid? classId,Guid? courseDesignId,int pageNum,int pageSize)
        {
            if (classId == null) return new PageOutput<DefenseRecordForClassAndStu>();
             List<DefenseRecordForClassAndStu> list = new List<DefenseRecordForClassAndStu>();
            //获取一个班级内的所有学生Id
            var students = await _studentEFRepository.GetAll().Where(c => c.ClassId == classId).ToListAsync();
            var designDefenses = await _designDefenseRecord.GetAll().WhereIf(courseDesignId != null, c => c.CourseDesignId == courseDesignId).ToListAsync();            foreach (var student in students)
            {
                var designDefense = designDefenses.FirstOrDefault(c => c.StudentId == student.Id);
                if (designDefense == null) continue;
                list.Add(new DefenseRecordForClassAndStu
                {
                    DefenseRecordId = designDefense.Id,
                    StudentName = student.Name,
                    State = designDefense.State,
                });
            }
            return new PageOutput<DefenseRecordForClassAndStu>(list, pageNum, pageSize);
        }
        #region 内部方法

        /// <summary>
        /// 生成答辩记录表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task<string> CreateWord(DesignDefenseRecord entities, string Type)
        {
            string tempFilePath = _env.WebRootPath + @"/FileTemplate/软件项目管理答辩记录表.docx", outFilePath = "wwwroot/Files/DefenseRecord/" + Guid.NewGuid() + ".docx";
            // var entities = await _designDefenseRecord.GetAsync(id);
            var dicts = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(entities));
            if (!string.IsNullOrEmpty(entities.Members))
            {
                dicts.Remove("Members");
                // List<string> MemberList = new List<string>(entities.Members.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                List<string> MemberList = JsonConvert.DeserializeObject<List<string>>(entities.Members);
                for (int i = 0; i < MemberList.Count; i++)
                {
                    dicts.Add("Members" + i, MemberList[i]);
                }
            }
            if (dicts.ContainsKey("DefenseTime"))
            {
                if (entities.DefenseTime != null) dicts["DefenseTime"] = ((DateTime)entities.DefenseTime).ToString("yyyy-MM-dd");
                else dicts["DefenseTime"] = LocalTool.TimeFormatStr(entities.CreationTime, 4);
            }
            if (dicts.ContainsKey("DefenseScore"))
            {
                if (entities.DefenseScore > 0) dicts["DefenseScore"] = Convert.ToInt32(entities.DefenseScore).ToString();
                else dicts["DefenseScore"] = null;
            }
            WordHelp.Export(tempFilePath, outFilePath, dicts);
            if(Type != "add")
            entities.WordUrl = entities.WordUrl.Replace(ConfigurationManager.AppSettings["Path"], _env.WebRootPath);
            //删除历史文件
            if (!string.IsNullOrEmpty(entities.WordUrl) && File.Exists(entities.WordUrl))
            {
                File.Delete(entities.WordUrl);
            }
            return outFilePath.Replace("wwwroot", ConfigurationManager.AppSettings["Path"]);
        }
        #endregion
    }
}
