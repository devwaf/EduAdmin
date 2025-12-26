using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Notifications;
using EduAdmin.AppService.Homeworks.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.FileManagements.Dto;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Homeworks
{
    /// <summary>
    /// 作业
    /// </summary>
    [AbpAuthorize]
    public class HomeworkAppService : EduAdminAppServiceBase, IHomeworkAppService
    {
        private readonly IRepository<Homework, Guid> _homeworkEFRepository;
        private readonly IRepository<Course, Guid> _courseEFRepository;
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IRepository<StudentHomework, Guid> _studentHomeworkEFRepository;
        private readonly IRepository<Classes, Guid> _classesEFRepository;
        private readonly IRepository<ClassCourse, Guid> _classCourseEFRepository;
        private readonly IRepository<Teacher, Guid> _teacherEFRepository;
        private readonly IRepository<ScoreWeight, Guid> _scoreWeightEFRepository;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IRepository<UserNotificationInfo, Guid> _userNotificationInfoRepository;
        private readonly IRepository<TenantNotificationInfo, Guid> _tenantNotificationInfoRepository;
        private readonly IHostingEnvironment _env;
        public HomeworkAppService(
           IRepository<Homework, Guid> homeworkEFRepository,
           IRepository<Course, Guid> courseEFRepository,
           IRepository<Student, Guid> studentEFRepository,
           IRepository<StudentHomework, Guid> studentHomeworkEFRepository,
           IRepository<Classes, Guid> classesEFRepository,
           IRepository<ClassCourse, Guid> classCourseEFRepository,
           IRepository<Teacher, Guid> teacherEFRepository,
           IRepository<UserNotificationInfo, Guid> userNotificationInfoRepository,
           IRepository<TenantNotificationInfo, Guid> tenantNotificationInfoRepository,
           IRepository<ScoreWeight, Guid> scoreWeightEFRepository,
           IHostingEnvironment env,
        INotificationPublisher notificationPublisher)
        {
            _homeworkEFRepository = homeworkEFRepository;
            _courseEFRepository = courseEFRepository;
            _studentEFRepository = studentEFRepository;
            _studentHomeworkEFRepository = studentHomeworkEFRepository;
            _classesEFRepository = classesEFRepository;
            _notificationPublisher = notificationPublisher;
            _classCourseEFRepository = classCourseEFRepository;
            _teacherEFRepository = teacherEFRepository;
            _userNotificationInfoRepository = userNotificationInfoRepository;
            _tenantNotificationInfoRepository = tenantNotificationInfoRepository;
            _scoreWeightEFRepository = scoreWeightEFRepository;
            _env = env;
        }
        /// <summary>
        /// 获取老师所有作业名称
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<List<HomeworkShowDto>> GetAllHomework(Guid? courseId, Guid teacherId, string type)
        {
            List<HomeworkShowDto> list = new List<HomeworkShowDto>();
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync();
            //获取当前老师的所有课程
            var allCourse = await _courseEFRepository.GetAll().Where(c => c.TeacherId == teacherId)
                .Select(c => new SelectDto<Guid>(c.Id, c.Name)).ToListAsync();
            var courseIds = allCourse.Select(c => c.Value).ToList();
            //获取当前老师课程的所有作业
            var homeworkList = await _homeworkEFRepository.GetAll().WhereIf(!string.IsNullOrEmpty(type), c => c.Type == type)
                .WhereIf(courseId != null, c => c.CourseId == courseId)
                .Where(c => courseIds.Contains(c.CourseId)).ToListAsync();
            foreach (var item in homeworkList)
            {
                var courseName = (allCourse.FirstOrDefault(c => c.Value == item.CourseId))?.Label;
                var homeworkType = scoreWeights.FirstOrDefault(c => c.Id.ToString() == item.Type).Name;
                string closeing = null;
                if (item.ClosingDate != null) closeing = LocalTool.TimeFormatStr((DateTime)item.ClosingDate, 6);
                list.Add(new HomeworkShowDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Score = item.Score,
                    Type = homeworkType,
                    ClosingDate = closeing,
                    CreationTime = LocalTool.TimeFormatStr((DateTime)item.CreationTime, 6),
                    CourseName = courseName,
                    FileType = item.FileType
                });
            }
            return list;
        }

        /// <summary>
        /// 学生上传作业
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<bool> SubmitStuHomework(SubmitStuHomeworkDto input)
        {
            var stuHomework = await _studentHomeworkEFRepository.GetAsync(input.StuHomeworkId);
            var stu = await _studentEFRepository.GetAsync(stuHomework.StudentId);
            var cla = await _classesEFRepository.GetAsync(stu.ClassId);
            var homework = await _homeworkEFRepository.GetAsync(stuHomework.HomeworkId);
            var teacherId = (await _courseEFRepository.GetAsync(homework.CourseId)).TeacherId;
            var userId = (await _teacherEFRepository.GetAsync(teacherId)).UserId;
            stuHomework.FilePath = JsonConvert.SerializeObject(input.FileList);
            stuHomework.State = false;
            var identity = new UserIdentifier(null, userId);
            await _notificationPublisher.PublishAsync(
                    "NewStuHomework",
                    new MessageDto(cla.SchoolYear + cla.Major + cla.Name,LocalTool.TimeFormatStr(DateTime.Now,2),"待批改", "/examination/taskcorrect", stuHomework.Id),
                    severity: NotificationSeverity.Info,
                    userIds: new[] { identity }
                );
            return true;
        }
        /// <summary>
        /// 学生获取指定课程的所有作业
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<StuHomeworkAndCountRateDto> GetStudentAllHomework(Guid studentId, Guid courseId)
        {
            List<StudentAllHomeworkShowDto> list = new List<StudentAllHomeworkShowDto>();
            //所有作业
            var homeworkInfo = await _homeworkEFRepository.GetAllListAsync(c => c.CourseId == courseId);
            var homeworkInfoIds = homeworkInfo.Select(c => c.Id).ToList();
            //当前课程的所有作业
            var homeworks = await _studentHomeworkEFRepository.GetAllListAsync(c => c.StudentId == studentId && homeworkInfoIds.Contains(c.HomeworkId));
            var finishCount = 0;
            foreach (var homework in homeworks)
            {
                var hwork = homeworkInfo.FirstOrDefault(c => c.Id == homework.HomeworkId);
                FileDto files = null;
                if (!string.IsNullOrEmpty(homework.FilePath))
                    files = JsonConvert.DeserializeObject<FileDto>(homework.FilePath);
                if (homework.State == null)
                {
                    list.Add(new StudentAllHomeworkShowDto
                    {
                        StuHomeworkId = homework.Id,
                        State = homework.State,
                        HomeworkName = hwork?.Name,
                        FileType = hwork?.FileType,
                        FilePath = files
                    });
                }
                if (homework.State != null)
                    finishCount++;
            }
            return new StuHomeworkAndCountRateDto
            {
                Rate = finishCount + "/" + homeworks.Count(),
                StuAllHomework = list
            };
        }
        /// <summary>
        /// 学生获取指定课程的所有作业历史
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<List<StudentHomeworkHistory>> GetStudentAllHomeworkHistory(Guid studentId, Guid courseId, bool state)
        {
            List<StudentHomeworkHistory> list = new List<StudentHomeworkHistory>();
            //所有作业
            var homeworkInfo = await _homeworkEFRepository.GetAllListAsync(c => c.CourseId == courseId);
            var homeworkInfoIds = homeworkInfo.Select(c => c.Id).ToList();
            var courses = await _courseEFRepository.GetAllListAsync();//所有课程
            //当前课程的所有作业
            var homeworks = await _studentHomeworkEFRepository.GetAllListAsync(c => c.StudentId == studentId && homeworkInfoIds.Contains(c.HomeworkId));
            foreach (var homework in homeworks)
            {
                var hwork = homeworkInfo.FirstOrDefault(c => c.Id == homework.HomeworkId);
                var course = courses.FirstOrDefault(c=>c.Id == hwork?.CourseId);
                if (homework.State == state)
                {
                    list.Add(new StudentHomeworkHistory
                    {
                        HomeworkName = hwork?.Name,
                        Score = homework?.Score,
                        Remark = homework?.Remark,
                        CourseName = course?.Name,
                        UpdateTime = LocalTool.TimeFormatStr((DateTime)homework.LastModificationTime, 2)
                    }); ;
                }
            }
            return list;
        }
        /// <summary>
        /// 获取作业对应班级的学生详情
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="classId"></param>
        /// <param name="type"></param>
        /// <param name="HomeworkId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<StudentHomeworkAndRate> GetAllStuHomeworkDetail(Guid courseId, Guid? classId, string type, Guid? HomeworkId, bool? state)
        {
            List<StudentHomeworkShowDto> list = new List<StudentHomeworkShowDto>();
            //大纲
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            //权重
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            var classNames = await _classesEFRepository.GetAllListAsync();
            //该班级的学生
            var clas = await _classCourseEFRepository.GetAll().Where(c => c.CourseId == courseId).Select(c => c.ClassId).ToListAsync();
            var stus = await _studentEFRepository.GetAll()
                .Where(c => clas.Contains(c.ClassId))
                .WhereIf(classId != null, c => c.ClassId == classId).ToListAsync();
            var homeworks = await _homeworkEFRepository.GetAll().Where(c => c.CourseId == courseId).Where(c => c.Type == type && c.FileType != "纸质").ToListAsync();
            var homeworkIds = homeworks.Select(c => c.Id).ToList();
            //指定作业类型的所有学生作业
            var stuhomeworks = await _studentHomeworkEFRepository.GetAllListAsync(c => homeworkIds.Contains(c.HomeworkId));
            var courseName = (await _courseEFRepository.GetAsync(courseId)).Name;
            int finish = 0;
            int num = 0;
            foreach (var student in stus)
            {
                List<StuHomework> list1 = new List<StuHomework>();
                var cla = classNames.FirstOrDefault(c => c.Id == student.ClassId);
                //当前学生某个作业类型的所有作业Id
                var stuhomeworkList = stuhomeworks.Where(c => c.StudentId == student.Id).ToList();
                foreach (var item in stuhomeworkList)
                {
                    var hw = homeworks.FirstOrDefault(c => c.Id == item.HomeworkId);
                    var scoreWeight = scoreWeights.FirstOrDefault(c => c.Id.ToString() == hw.Type);
                    list1.Add(new StuHomework
                    {
                        Id = item.Id,
                        HomeworkId = hw.Id,
                        HomeworkName = scoreWeight.Name + hw.Times.ToString(),
                        HomeworkScore = item.Score,
                        State = item.State,
                    });
                    if (item.State == true) finish++;
                    num++;
                }
                var flag = new List<StuHomework>();
                if (HomeworkId != null && state != null)
                {
                    var selectHome = homeworks.FirstOrDefault(c => c.Id == HomeworkId);
                    var sameHomeworkIds = homeworks.Where(c => c.CourseId == selectHome.CourseId && c.Times == selectHome.Times && c.Type == selectHome.Type).Select(c => c.Id).ToList();//班级不同其他都相同的作业
                    var sameStuHomeworkIds = stuhomeworks.Where(c => c.StudentId == student.Id && sameHomeworkIds.Contains(c.HomeworkId)).Select(c => c.HomeworkId).ToList();//所有作业类型
                    flag = list1.Where(c => sameStuHomeworkIds.Contains(c.HomeworkId) && c.State == state).ToList();//拥有作业状态为state的作业列表 //拥有指定作业状态的学生才显示
                    if (flag.Count() > 0)
                    {
                        list.Add(new StudentHomeworkShowDto
                        {
                            StuId = student.Id,
                            StuName = student.Name,
                            Gender = student.Gender,
                            CourseName = courseName,
                            ClassName = cla.SchoolYear + cla.Major + cla.Name,
                            Sno = student.Sno,
                            StuHomeworks = list1
                        });
                    }
                }
                else
                {
                    list.Add(new StudentHomeworkShowDto
                    {
                        StuId = student.Id,
                        StuName = student.Name,
                        Gender = student.Gender,
                        CourseName = courseName,
                        ClassName = cla.SchoolYear + cla.Major + cla.Name,
                        Sno = student.Sno,
                        StuHomeworks = list1
                    });
                }
            }
            return new StudentHomeworkAndRate
            {
                Rate = finish + "/" + num,
                StudentHomeworks = list
            };
        }
        /// <summary>
        /// 老师发布作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ResultDto> AddHomework(CreateHomeworkDto input)
        {
            bool? state = null;
            if (input.FileType == "纸质")
                state = true;
            var classes = await _classCourseEFRepository.GetAllListAsync(c => c.CourseId == input.CourseId);
            foreach (var item in classes)
            {
                var students = await _studentEFRepository.GetAllListAsync(c => c.ClassId == item.ClassId);

                var homework = new Homework
                {
                    ClassesId = item.ClassId,
                    ClosingDate = input.ClosingDate,
                    CourseId = input.CourseId,
                    FileType = input.FileType,
                    Times = input.Times,
                    Name = input.Name,
                    Type = input.Type
                };
                var id = await _homeworkEFRepository.InsertAndGetIdAsync(homework);
                UserIdentifier[] MyArray = new UserIdentifier[students.Count];
                //发布作业的时候把所有学生与作业对应起来
                //int index = 0;
                foreach (var student in students)
                {
                   var stuHomeworkId = await _studentHomeworkEFRepository.InsertAndGetIdAsync(new StudentHomework
                    {
                        StudentId = student.Id,
                        State = state,
                        HomeworkId = id,
                    });
                    //MyArray[index] = new UserIdentifier(null, student.UserId);
                    //index++;
                    if (input.IsNotSendMessage == false)
                    {
                        await _notificationPublisher.PublishAsync(
                            "NewHomework",
                            new MessageDto(input.Name, LocalTool.TimeFormatStr(DateTime.Now, 2), "待批改", "/curriculum/fill ", stuHomeworkId),
                            severity: NotificationSeverity.Info,
                            userIds: new[] { new UserIdentifier(null, student.UserId) }
                        );
                    }
                }
            }
            return new ResultDto(true, "发布成功");
        }
        /// <summary>
        /// 批改作业
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<UpdateResult> UpdateStudentHomeWork(UpdateHomeworkDto input)
        {
            if (input.Score < 0 || input.Score > 100)
            {
                return new UpdateResult("分数超出正常范围");
            }
            var stuHomework = await _studentHomeworkEFRepository.GetAsync(input.Id);
            var homework = await _homeworkEFRepository.FirstOrDefaultAsync(c => c.Id == stuHomework.HomeworkId);
            var student = await _studentEFRepository.FirstOrDefaultAsync(c => c.Id == stuHomework.StudentId);
            stuHomework.Remark = input.Remark;
            stuHomework.Score = input.Score;
            stuHomework.State = true;
            await DeleteHomeworkMessage(stuHomework.Id,AbpSession.UserId);
            await _notificationPublisher.PublishAsync(
                        "NewHomework",
                        new MessageDto(homework.Name, LocalTool.TimeFormatStr(DateTime.Now, 2), "已批改", "/curriculum/fill ", stuHomework.Id),
                        severity: NotificationSeverity.Info,
                        userIds: new[] {new UserIdentifier(null, student.UserId) }
                    );
            return new UpdateResult();
        }
        /// <summary>
        /// 获取作业批改
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<UpdateHomeworkDto> GetStudentHomeWorkScore(Guid id)
        {
            var stuHomework = await _studentHomeworkEFRepository.GetAsync(id);
            return new UpdateHomeworkDto
            {
                Id = id,
                Score = stuHomework.Score,
                Remark = stuHomework.Remark
            };
        }
        /// <summary>
        /// 获取学生作业文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<string> GetStudentHomework(Guid id)
        {
            var stuHome = await _studentHomeworkEFRepository.GetAsync(id);
            if (stuHome.FilePath == null) return null;
            var fileDto = JsonConvert.DeserializeObject<FileDto>(stuHome.FilePath);
            var outputPath = fileDto.Path.Replace(ConfigurationManager.AppSettings["Path"], _env.WebRootPath);
            // pdf的文件夹地址
            var pdfPath = Path.Combine(_env.WebRootPath, "PDFs", "StudentHomeworks");
            if (!Directory.Exists(pdfPath))
            {
                Directory.CreateDirectory(pdfPath);
            }
            var pdf = OfficeConvert.GetFilePdf(outputPath, pdfPath, _env.WebRootPath, LocalTool.GetAppSettings("path"));
            return pdf;
        }
        /// <summary>
        /// 修改作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<UpdateResult> UpdateHomework(UpdateTHomeworkDto input)
        {
            var homework = ObjectMapper.Map<Homework>(input);
            await _homeworkEFRepository.UpdateAsync(homework);
            return new UpdateResult();
        }
        /// <summary>
        /// 删除作业
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<DeleteResult> DeleteHomework(Guid id)
        {
            await _homeworkEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
        /// <summary>
        /// 获取学生的所有消息
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<List<HomeworkMessageDto>> GetAllHomeworkMessage(Guid userId)
        {
            List<HomeworkMessageDto> list = new List<HomeworkMessageDto>();
            var student = await _studentEFRepository.GetAsync(userId);
            var x = await _userNotificationInfoRepository.GetAllListAsync(c => c.State == UserNotificationState.Unread && c.UserId == student.UserId);
            //获取当前学生未读的所有消息
            var noticeId = await _userNotificationInfoRepository.GetAll()
                .Where(c => c.State == UserNotificationState.Unread && c.UserId == student.UserId)
                .Select(c => c.TenantNotificationId).ToListAsync();
            var noticeInfos = await _tenantNotificationInfoRepository.GetAll().Where(c => noticeId.Contains(c.Id)).OrderByDescending(c => c.CreationTime).ToListAsync();
            foreach (var noticeInfo in noticeInfos)
            {
                var message = JsonConvert.DeserializeObject<MessageDto>(noticeInfo.Data);
                list.Add(new HomeworkMessageDto
                {
                    NoticeId = noticeInfo.Id,
                    Message = message.Content,
                    State = message.State,
                    CreationTime = message.CreationTime,
                    Router = message.Router,
                });
            }
            return list;
        }
        /// <summary>
        /// 获取老师的所有消息
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<List<HomeworkTeaMessageDto>> GetAllTeaHomeworkMessage(Guid userId)
        {
            List<HomeworkTeaMessageDto> list = new List<HomeworkTeaMessageDto>();
            List<MessageDto> list1 = new List<MessageDto>();
            var tea = await _teacherEFRepository.GetAsync(userId);
            //获取当前老师未读的所有消息
            var notice = await _userNotificationInfoRepository.GetAll()
                .Where(c => c.State == UserNotificationState.Unread && c.UserId == tea.UserId)
                .ToListAsync();
            var noticeId = notice.Select(c => c.TenantNotificationId).ToList();
            var notices = await _tenantNotificationInfoRepository.GetAll().Where(c => noticeId.Contains(c.Id)).ToListAsync();
            var noticeDatas = notices.Select(c => c.Data).ToList();
            foreach (var noticeData in noticeDatas)
            {
                list1.Add(JsonConvert.DeserializeObject<MessageDto>(noticeData));
            }
            var noticeInfos = noticeDatas.Distinct().ToList();
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy.MM.dd";
            foreach (var noticeInfo in noticeInfos)
            {
                var message = JsonConvert.DeserializeObject<MessageDto>(noticeInfo);
                var noticeDetail = list1.Where(c=>c.Content == message.Content).ToList();
                var creationTime = noticeDetail.Select(c => Convert.ToDateTime(c.CreationTime,dtFormat)).Max();
                list.Add(new HomeworkTeaMessageDto
                {
                    count = noticeDetail.Count(),
                    CreationTime = LocalTool.TimeFormatStr(creationTime,2),
                    State = message.State,
                    Message = message.Content,
                    Router = message.Router
                });
            }
            return list.Distinct(new CompareDto()).ToList();
        }
        /// <summary>
        /// 老师消息状态改为已读
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<ResultDto> DeleteTeaHomeworkMessage(Guid userId, string message)
        {
            List<Guid> list = new List<Guid>();
            var tea = await _teacherEFRepository.GetAsync(userId);
            //获取当前老师未读的所有消息
            var notice = await _userNotificationInfoRepository.GetAll()
                .Where(c => c.State == 0 && c.UserId == tea.UserId)
                .ToListAsync();
            var noticeId = notice.Select(c => c.TenantNotificationId).ToList();
            var notices = await _tenantNotificationInfoRepository.GetAll().Where(c => noticeId.Contains(c.Id))
                .ToListAsync();
            foreach (var noticeInfo in notices)
            {
                var mes = JsonConvert.DeserializeObject<MessageDto>(noticeInfo.Data);
                if (mes.Content == message)
                {
                    list.Add(noticeInfo.Id);
                }
            }
            var userNotices = notice.Where(c => list.Contains(c.TenantNotificationId)).ToList();
            foreach (var userNotice in userNotices)
            {
                userNotice.State = UserNotificationState.Read;
                await _userNotificationInfoRepository.UpdateAsync(userNotice);
            }
            return new ResultDto(true, "已读");
        }
        /// <summary>
        /// 消息状态改为已读
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        private async Task<ResultDto> DeleteHomeworkMessage(Guid workId,long? userId)
        {
            var userNotices = await _userNotificationInfoRepository.GetAllListAsync(c=>c.UserId == userId);
            var userNoticeIds = userNotices.Select(c => c.TenantNotificationId).ToList();
            var notices = await _tenantNotificationInfoRepository.GetAllListAsync(c => userNoticeIds.Contains(c.Id));
            foreach (var noticeInfo in notices)
            {
                var mes = JsonConvert.DeserializeObject<MessageDto>(noticeInfo.Data);
                if (mes.WorkId == workId)
                {
                  var userNotice =   userNotices.FirstOrDefault(c => c.TenantNotificationId == noticeInfo.Id);
                    userNotice.State = UserNotificationState.Read;
                    await _userNotificationInfoRepository.UpdateAsync(userNotice);
                }
            }
            //userNotice
            
            return new ResultDto(true, "已读");
        }
    }
}
