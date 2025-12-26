using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Notifications;
using EduAdmin.AppService.Homeworks.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.FileManagements;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.GraduationDesigns
{
    /// <summary>
    /// 教师
    /// </summary>
    [AbpAuthorize]
    public class GraduationDesignAppService : EduAdminAppServiceBase, IGraduationDesignAppService
    {
        private readonly IRepository<GraduationDesign, Guid> _graduationDesignEFRepository;
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IRepository<Classes, Guid> _classesEFRepository;
        private readonly IFileManagementAppService _fileManagementAppService;
        private readonly INotificationPublisher _notificationPublisher;
        public GraduationDesignAppService(
           IRepository<GraduationDesign, Guid> graduationDesignEFRepository,
           IRepository<Student, Guid> studentEFRepository,
           IRepository<Classes, Guid> classesEFRepository,
           IFileManagementAppService fileManagementAppService,
           INotificationPublisher notificationPublisher)
        {
            _graduationDesignEFRepository = graduationDesignEFRepository;
            _studentEFRepository = studentEFRepository;
            _classesEFRepository = classesEFRepository;
            _fileManagementAppService = fileManagementAppService;
            _notificationPublisher = notificationPublisher;
        }
        /// <summary>
        /// 老师获取所管理学生的所有毕业设计
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<List<GraduationDesignShowDto>> GetAllGraduationDesign(Guid teacherId)
        {
            List<GraduationDesignShowDto> list = new List<GraduationDesignShowDto>();
            var students = await _studentEFRepository.GetAllListAsync(c => c.GraDesTeacherId == teacherId);//获取该老师管理的所有学生
            var classes = await _classesEFRepository.GetAllListAsync();
            var graDesins = await _graduationDesignEFRepository.GetAllListAsync();
            foreach(var student in students)
            {
                var cla = classes.FirstOrDefault(c => c.Id == student.ClassId);
                var graDes = graDesins.FirstOrDefault(c => c.Id == student.GraduationDesignId);//该学生的毕业设计
                ShowGraFileDto annrx1 = null, secondReport1 = null, assignment1 = null, checkReport1 = null, dissertation1 = null, headline1 = null, firstReport1 = null, foreignTrans1 = null, draftDissertation1 = null;
                if (graDes?.Annex != null)
                {
                    var annrx = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Annex);
                    annrx1 = new ShowGraFileDto
                    {
                        State = annrx.State,
                        Url = annrx.FilePath?.Path
                    };
                }
                else
                {
                    annrx1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.SecondReport != null)
                {
                    var secondReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.SecondReport);
                    secondReport1 = new ShowGraFileDto
                    {
                        State = secondReport.State,
                        Url = secondReport.FilePath?.Path
                    };
                }
                else
                {
                    secondReport1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.Assignment != null)
                {
                    var assignment = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Assignment);
                    assignment1 = new ShowGraFileDto
                    {
                        State = assignment.State,
                        Url = assignment.FilePath?.Path
                    };
                }
                else
                {
                    assignment1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.CheckReport != null)
                {
                    var checkReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.CheckReport);
                    checkReport1 = new ShowGraFileDto
                    {
                        State = checkReport.State,
                        Url = checkReport.FilePath?.Path
                    };
                }
                else
                {
                    checkReport1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.Dissertation != null)
                {
                    var dissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Dissertation);
                    dissertation1 = new ShowGraFileDto
                    {
                        State = dissertation.State,
                        Url = dissertation.FilePath?.Path
                    };
                }
                else
                {
                    dissertation1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.Headline != null)
                {
                    var headline = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Headline);
                    headline1 = new ShowGraFileDto
                    {
                        State = headline.State,
                        Url = headline.FilePath?.Path
                    };
                }
                else
                {
                    headline1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.FirstReport != null)
                {
                    var firstReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.FirstReport);
                    firstReport1 = new ShowGraFileDto
                    {
                        State = firstReport.State,
                        Url = firstReport.FilePath?.Path
                    };
                }
                else
                {
                    firstReport1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.ForeignTrans != null)
                {
                    var foreignTrans = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.ForeignTrans);
                    foreignTrans1 = new ShowGraFileDto
                    {
                        State = foreignTrans.State,
                        Url = foreignTrans.FilePath?.Path
                    };
                }
                else
                {
                    foreignTrans1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                if (graDes?.DraftDissertation != null)
                {
                    var draftDissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.DraftDissertation);
                    draftDissertation1 = new ShowGraFileDto
                    {
                        State = draftDissertation.State,
                        Url = draftDissertation.FilePath?.Path
                    };
                }
                else
                {
                    draftDissertation1 = new ShowGraFileDto
                    {
                        State = null,
                        Url = null
                    };
                }
                list.Add(new GraduationDesignShowDto
                {
                    StuId = student.Id,
                    Sno = student.Sno,
                    StuName = student.Name,
                    Gender = student.Gender,
                    Classses = cla.SchoolYear + "级" + cla.Major + cla.Name + "班",
                    Name = graDes?.Name,
                    SecondReport = secondReport1,
                    Annex = annrx1,
                    Assignment = assignment1,
                    Dissertation = dissertation1,
                    CheckReport = checkReport1,
                    DraftDissertation = draftDissertation1,
                    FirstReport = firstReport1,
                    ForeignTrans = foreignTrans1,
                    Headline = headline1,
                    State = graDes?.State,
                    Id = graDes?.Id,
                });
            }
            return list;
        }
        /// <summary>
        /// 学生获取的毕业设计
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<GraduationDesignStuShowDto> GetGraduationDesign(Guid studentId)
        {
           var student = await _studentEFRepository.GetAsync(studentId);
            bool isStart = true; 
            if(student.GraDesTeacherId == null)
            {
                isStart = false;
            }
           var cla = await _classesEFRepository.GetAsync(student.ClassId);
            GraduationDesign graDes = new GraduationDesign();
            if (student.GraduationDesignId != null)
            {
                graDes = await _graduationDesignEFRepository.GetAsync((Guid)student.GraduationDesignId);
            }
            ShowGraFileDto annrx1 = null, secondReport1 = null, assignment1 = null, checkReport1 = null, dissertation1 = null, headline1 = null, firstReport1 = null, foreignTrans1 = null, draftDissertation1 = null;
            if (graDes.Annex != null)
            {
                var annrx = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Annex);
                annrx1 = new ShowGraFileDto
                {
                    State = annrx.State,
                    Url = annrx.FilePath?.FileName
                };
            }
            else
            {
                annrx1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.SecondReport != null)
            {
                var secondReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.SecondReport);
                secondReport1 = new ShowGraFileDto
                {
                    State = secondReport.State,
                    Url = secondReport.FilePath?.FileName
                };
            }
            else
            {
                secondReport1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.Assignment != null)
            {
                var assignment = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Assignment);
                assignment1 = new ShowGraFileDto
                {
                    State = assignment.State,
                    Url = assignment.FilePath?.FileName
                };
            }
            else
            {
                assignment1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.CheckReport != null)
            {
                var checkReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.CheckReport);
                checkReport1 = new ShowGraFileDto
                {
                    State = checkReport.State,
                    Url = checkReport.FilePath?.FileName
                };
            }
            else
            {
                checkReport1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.Dissertation != null)
            {
                var dissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Dissertation);
                dissertation1 = new ShowGraFileDto
                {
                    State = dissertation.State,
                    Url = dissertation.FilePath?.FileName
                };
            }
            else
            {
                dissertation1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.Headline != null)
            {
                var headline = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Headline);
                headline1 = new ShowGraFileDto
                {
                    State = headline.State,
                    Url = headline.FilePath?.FileName
                };
            }
            else
            {
                headline1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.FirstReport != null)
            {
                var firstReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.FirstReport);
                firstReport1 = new ShowGraFileDto
                {
                    State = firstReport.State,
                    Url = firstReport.FilePath?.FileName
                };
            }
            else
            {
                firstReport1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.ForeignTrans != null)
            {
                var foreignTrans = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.ForeignTrans);
                foreignTrans1 = new ShowGraFileDto
                {
                    State = foreignTrans.State,
                    Url = foreignTrans.FilePath?.FileName
                };
            }
            else
            {
                foreignTrans1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            if (graDes.DraftDissertation != null)
            {
                var draftDissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.DraftDissertation);
                draftDissertation1 = new ShowGraFileDto
                {
                    State = draftDissertation.State,
                    Url = draftDissertation.FilePath?.FileName
                };
            }
            else
            {
                draftDissertation1 = new ShowGraFileDto
                {
                    State = null,
                    Url = null
                };
            }
            Guid? greDesId = graDes.Id;
            if(graDes.Id == Guid.Empty)
            {
                greDesId = null;
            }
            return new GraduationDesignStuShowDto
            {
                Id = greDesId,
                State = graDes.State,
                Name = graDes.Name,
                SecondReport = secondReport1,
                Annex = annrx1,
                Assignment = assignment1,
                Dissertation = dissertation1,
                CheckReport = checkReport1,
                DraftDissertation = draftDissertation1,
                FirstReport = firstReport1,
                ForeignTrans = foreignTrans1,
                Headline = headline1,
                IsStart = isStart
            };
        }
        /// <summary>
        /// 学生上传毕业设计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<ResultDto> AddGraduationDesign(CreateGraduationDesignDto input)
        {
            var stu = await _studentEFRepository.GetAsync(input.StuId);//学生

            if (stu.GraduationDesignId == null)
            {
                var graduationDesign = ObjectMapper.Map<GraduationDesign>(input);
                var id = await _graduationDesignEFRepository.InsertAndGetIdAsync(graduationDesign);
                stu.GraduationDesignId = id;
                return new ResultDto(true, "sucess");
            }
            else
            {
                var gra = await _graduationDesignEFRepository.GetAsync(input.Id);
                var grades = ObjectMapper.Map<GraduationDesign>(input);
                if (grades.Annex != null)
                    gra.Annex = grades.Annex;
                if (grades.SecondReport != null)
                    gra.SecondReport = grades.SecondReport;
                if (grades.Assignment != null)
                    gra.Assignment = grades.Assignment;
                if (grades.CheckReport != null)
                    gra.CheckReport = grades.CheckReport;
                if (grades.Dissertation != null)
                    gra.Dissertation = grades.Dissertation;
                if (grades.Headline != null)
                    gra.Headline = grades.Headline;
                if (grades.FirstReport != null)
                    gra.FirstReport = grades.FirstReport;
                if (grades.ForeignTrans != null)
                    gra.ForeignTrans = grades.ForeignTrans;
                if (grades.DraftDissertation != null)
                    gra.DraftDissertation = grades.DraftDissertation;
                return new ResultDto(true,"sucess");
            }
        }
        /// <summary>
        /// 老师打回毕业设计
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ResultDto> DeleteGraduationDesign(Guid id,string fileName)
        {
            var student = await _studentEFRepository.FirstOrDefaultAsync(c=>c.GraduationDesignId == id);
            var userId = student.UserId;
            string message = null;
            var graDes = await _graduationDesignEFRepository.GetAsync(id);
            Guid fileId = Guid.Empty;
            if(fileName == "assignment")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Assignment);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.Assignment = newFile;
                message = "任务书（毕业设计）已退回";
            }
            if (fileName == "annex")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Annex);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.Annex = newFile;
                message = "附件（毕业设计）已退回";
            }
            if (fileName == "checkReport")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.CheckReport);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.CheckReport = newFile;
                message = "查重报告（毕业设计）已退回";
            }
            if (fileName == "dissertation")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Dissertation);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.Dissertation = newFile;
                message = "论文（毕业设计）已退回";
            }
            if (fileName == "draftDissertation")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.DraftDissertation);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.DraftDissertation = newFile;
                message = "论文草稿（毕业设计）已退回";
            }
            if (fileName == "firstReport")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.FirstReport);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.FirstReport = newFile;
                message = "第一阶段情况报告（毕业设计）已退回";
            }
            if (fileName == "foreignTrans")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.ForeignTrans);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.ForeignTrans = newFile;
                message = "外文翻译（毕业设计）已退回";
            }
            if (fileName == "headline")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.Headline);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.Headline = newFile;
                message = "开题报告（毕业设计）已退回";
            }
            if (fileName == "secondReport")
            {
                var file = JsonConvert.DeserializeObject<GraDsignFileAndState>(graDes.SecondReport);
                file.State = false;
                await _fileManagementAppService.ForceDeleteFile(file.FilePath.Id);
                file.FilePath = null;
                var newFile = JsonConvert.SerializeObject(file);
                graDes.SecondReport = newFile;
                message = "第二阶段情况报告（毕业设计）已退回";
            }
            var identity = new UserIdentifier(null, userId);
            await _notificationPublisher.PublishAsync(
                "graduationDesign",
                new MessageDto(message,LocalTool.TimeFormatStr(DateTime.Now,2),"已退回", "/graduation",fileId),
                severity: NotificationSeverity.Info,
                userIds: new UserIdentifier[] { identity });
            return new ResultDto(true, "打回成功");
        }
    }
}
