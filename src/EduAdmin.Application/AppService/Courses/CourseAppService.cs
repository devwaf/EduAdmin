using Abp.Authorization;
using Abp.Domain.Repositories;
using EduAdmin.AppService.Courses.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.FileManagements;
using EduAdmin.LocalTools.Dto;
using EduAdmin.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Courses
{
    /// <summary>
    /// 教师
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class CourseAppService : EduAdminAppServiceBase, ICourseAppService
    {
        private readonly IRepository<Teacher, Guid> _teacherEFRepository;
        private readonly IRepository<Course, Guid> _courseEFRepository;
        private readonly IRepository<ClassCourse, Guid> _classCourseEFRepository;
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IRepository<StudentCourse, Guid> _studentCourseEFRepository;
        private readonly IRepository<Classes, Guid> _classesEFRepository;
        private readonly IRepository<Homework, Guid> _homeworkEFRepository;
        private readonly IRepository<StudentHomework, Guid> _studentHomeworkEFRepository;
        private readonly IRepository<Outline, Guid> _outlineHomeworkEFRepository;
        private readonly IRepository<QuestionScore, Guid> _questionScoreEFRepository;
        private readonly IRepository<Question, Guid> _questionEFRepository;
        private readonly IRepository<ScoreWeight, Guid> _scoreweightEFRepository;
        private readonly IFileManagementAppService _fileManagementAppService;
        private readonly IUserAppService _userAppService;
        public CourseAppService(
            IRepository<ScoreWeight, Guid> scoreweightEFRepository,
            IRepository<Question, Guid> questionEFRepository,
        IRepository<Teacher, Guid> teacherEFRepository,
            IRepository<Course, Guid> courseEFRepository,
            IRepository<ClassCourse, Guid> classCourseEFRepository,
            IRepository<Student, Guid> studentEFRepository,
            IRepository<StudentCourse, Guid> studentCourseEFRepository,
            IRepository<Classes, Guid> classesEFRepository,
            IRepository<Outline, Guid> outlineHomeworkEFRepository,
            IUserAppService userAppService,
            IRepository<Homework, Guid> homeworkEFRepository,
            IRepository<StudentHomework, Guid> studentHomeworkEFRepository,
            IRepository<QuestionScore, Guid> questionScoreEFRepository,
            IFileManagementAppService fileManagementAppService)
        {
            _teacherEFRepository = teacherEFRepository;
            _courseEFRepository = courseEFRepository;
            _classCourseEFRepository = classCourseEFRepository;
            _studentEFRepository = studentEFRepository;
            _studentCourseEFRepository = studentCourseEFRepository;
            _fileManagementAppService = fileManagementAppService;
            _classesEFRepository = classesEFRepository;
            _homeworkEFRepository = homeworkEFRepository;
            _userAppService = userAppService;
            _studentHomeworkEFRepository = studentHomeworkEFRepository;
            _outlineHomeworkEFRepository = outlineHomeworkEFRepository;
            _questionScoreEFRepository = questionScoreEFRepository;
            _questionEFRepository = questionEFRepository;
            _scoreweightEFRepository = scoreweightEFRepository;
        }
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns></returns>
        public async Task<List<CourseShowDto>> GetAllCourse()
        {
            var id = AbpSession.UserId;
            if (id == null) return null;
            var teacher = await _teacherEFRepository.FirstOrDefaultAsync(c => c.UserId == id);
            var courseList = await _courseEFRepository.GetAllListAsync(c =>c.Kind == "课程" && c.TeacherId == teacher.Id);
            return ObjectMapper.Map<List<CourseShowDto>>(courseList);
        }
        /// <summary>
        /// 获取课设列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectDto<Guid>>> GetAllDesginSelect()
        {
            var course = await GetAllDesgin();
            return course.Select(c=>new SelectDto<Guid>(c.Id,c.Name)).ToList();
        }
        /// <summary>
        /// 获取所有课设
        /// </summary>
        /// <returns></returns>
        public async Task<List<CourseShowDto>> GetAllDesgin()
        {
            var id = AbpSession.UserId;
            if (id == null) return null;
            var teacher = await _teacherEFRepository.FirstOrDefaultAsync(c => c.UserId == id);
            var courseList = await _courseEFRepository.GetAllListAsync(c => c.Kind == "课设" && c.TeacherId == teacher.Id);
            return ObjectMapper.Map<List<CourseShowDto>>(courseList);
        }
        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectDto<Guid>>> GetAllCourseSelect()
        {
            var course = await GetAllCourse();
            return course.Select(c => new SelectDto<Guid>(c.Id, c.Name)).ToList();
        }
        /// <summary>
        /// 获取当前老师Id
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Guid> GetCurrentUserId()
        {
            //获取当前角色
            var tid = AbpSession.UserId;
            if (tid == null) throw new Exception("请先登录");
            //给课程添加老师Id
            var teacher = await _teacherEFRepository.FirstOrDefaultAsync(c => c.UserId == tid);
            return teacher.Id;
        }
        /// <summary>
        /// 添加课程课设
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddCourse(CreateCourseDto input)
        {
            input.TeacherId = await GetCurrentUserId();
            var course = ObjectMapper.Map<Course>(input);
            var id = await _courseEFRepository.InsertAndGetIdAsync(course);
            var allStudent = await _studentEFRepository.GetAllListAsync();
            //添加课程时把课程和班级对应起来
            foreach (var item in input.ClassIds)
            {
                await _classCourseEFRepository.InsertAndGetIdAsync(new ClassCourse
                {
                    ClassId = item,
                    CourseId = id
                });
                var stus = allStudent.Where(c => c.ClassId == item).ToList();
                //给班级里的所有学生绑定课程
                if (stus.Count > 0)
                    foreach (var student in stus)
                    {
                        await _studentCourseEFRepository.InsertAndGetIdAsync(new StudentCourse
                        {
                            StudentId = student.Id,
                            CourseId = id
                        });
                    }
            }
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改课程课设
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateCourse(CreateCourseDto input)
        {
            input.TeacherId = await GetCurrentUserId();
            //原有的班级
            var oldClassIds = await _classCourseEFRepository.GetAll().Where(c => c.CourseId == input.Id).Select(c=>c.ClassId).ToListAsync();
            var allStudent = await _studentEFRepository.GetAllListAsync();
            foreach (var item in oldClassIds)
            {
                var stuIds = allStudent.Where(c => c.ClassId == item).Select(c=>c.Id).ToList();
                if (!input.ClassIds.Contains(item))//之前有现在没有
                {
                    await _classCourseEFRepository.DeleteAsync(c => c.ClassId == item && c.CourseId == input.Id);//删除班级课程之前关联
                    await _studentCourseEFRepository.DeleteAsync(c=>c.CourseId == input.Id && stuIds.Contains(c.StudentId));
                }
            }
            foreach (var item in input.ClassIds)
            {
                var stus = allStudent.Where(c => c.ClassId == item).ToList();
                if (!oldClassIds.Contains(item))//之前没有现在有
                {
                    //添加课程时把课程和班级对应起来
                        await _classCourseEFRepository.InsertAndGetIdAsync(new ClassCourse
                        {
                            ClassId = item,
                            CourseId = input.Id
                        });
                        //给班级里的所有学生绑定课程
                        if (stus.Count > 0)
                            foreach (var student in stus)
                            {
                                await _studentCourseEFRepository.InsertAndGetIdAsync(new StudentCourse
                                {
                                    StudentId = student.Id,
                                    CourseId = input.Id
                                });
                            }
                }
            }
            var course = ObjectMapper.Map<Course>(input);
            await _courseEFRepository.UpdateAsync(course);

            return new UpdateResult();
        }
        /// <summary>
        /// 删除课程课设
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteCourse(Guid id)
        {
            //删除课程的所有班级关联
            await _classCourseEFRepository.DeleteAsync(c=>c.CourseId == id);
            
            var homework = await _homeworkEFRepository.GetAll().Where(c=>c.CourseId==id).Select(c=>c.Id).ToListAsync();
            //删除课程的所有学生作业
            await _studentHomeworkEFRepository.DeleteAsync(c => homework.Contains(c.HomeworkId));
            //删除所有发布的作业
            await _homeworkEFRepository.DeleteAsync(c=>c.CourseId == id);
            //删除学生与课程间的关系
            await _studentCourseEFRepository.DeleteAsync(c => c.CourseId == id);
            //删除课程
            await _courseEFRepository.DeleteAsync(id);
            //删除所有学生作业的成绩
            await _questionScoreEFRepository.DeleteAsync(c=>c.CourseId == id);
            return new DeleteResult();
        }
        /// <summary>
        /// 获取所有的课程名称（不重复）
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetAllCourseName(Guid teacherId)
        {
          var courseName =  await _courseEFRepository.GetAll().Where(c => c.Kind == "课程").Where(c => c.TeacherId == teacherId).Select(c=>new SelectDto<string>(c.Name,c.Name)).Distinct().ToListAsync();
            return courseName;
        }
        /// <summary>
        /// 获取所有的学期（不重复）
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetAllTerm(Guid teacherId)
        {
            var courseName = await _courseEFRepository.GetAll().Where(c => c.Kind == "课程").Where(c => c.TeacherId == teacherId).Select(c =>new SelectDto<string>(c.Semester,c.Semester)).Distinct().ToListAsync();
            return courseName;
        }
        /// <summary>
        /// 课程可选学期
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public List<SelectDto<string>> GetTerm(Guid teacherId)
        {
            var date = DateTime.Now.Year.ToString();
            int res = int.Parse(date.Substring(date.Length-3, date.Length-1));
            return new List<SelectDto<string>>
            {
                new SelectDto<string>(res-1+"年-"+res+"年第一学期",res-1+"年-"+res+"年第一学期"),
                new SelectDto<string>(res-1+"年-"+res+"年第二学期",res-1+"年-"+res+"年第二学期"),
                new SelectDto<string>(res+"年-"+(res+1)+"年第一学期",res+"年-"+(res+1)+"年第一学期"),
                new SelectDto<string>(res+"年-"+(res+1)+"年第二学期",res+"年-"+(res+1)+"年第二学期"),
                new SelectDto<string>(res+1+"年-"+(res+2)+"年第一学期",res+1+"年-"+(res+2)+"年第一学期"),
                new SelectDto<string>(res+1+"年-"+(res+2)+"年第二学期",res+1+"年-"+(res+2)+"年第二学期")
            };
        }
        /// <summary>
        /// 获取该课程名称有哪几个学期
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetTermForCourseName(string courseName)
        {   
          var allTerm = await _courseEFRepository.GetAll().Where(c => c.Kind == "课程").Where(c => c.Name == courseName).Select(c=> new SelectDto<string>(c.Semester, c.Semester)).Distinct().ToListAsync();
            return allTerm;
        }
        /// <summary>
        /// 根据课程名称和学期得到课程Id
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="semester"></param>
        /// <returns></returns>
        public async Task<Guid> GetCourseSelect(string courseName,string semester)
        {
          var course =  await _courseEFRepository.FirstOrDefaultAsync(c=>c.Name == courseName && c.Semester == semester);
            return course.Id;
        }
        /// <summary>
        /// 获取拥有该学期的课程
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetCourseNameForTrem(string semester)
        {
            var courseNames = await _courseEFRepository.GetAll().Where(c => c.Kind == "课程").Where(c=>c.Semester == semester).Select(c => new SelectDto<string>(c.Name, c.Name)).Distinct().ToListAsync();
            return courseNames;
        }
        /// <summary>
        /// 获取所有的课设名称（不重复）
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetAllDesignName(Guid teacherId)
        {
            var courseName = await _courseEFRepository.GetAll().Where(c => c.Kind == "课设").Where(c => c.TeacherId == teacherId).Select(c => new SelectDto<string>(c.Name, c.Name)).Distinct().ToListAsync();

            return courseName;
        }
        /// <summary>
        /// 获取所有的课设学期（不重复）
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetAllDesignTerm(Guid teacherId)
        {
            var courseName = await _courseEFRepository.GetAll().Where(c => c.Kind == "课设").Where(c => c.TeacherId == teacherId).Select(c => new SelectDto<string>(c.Semester, c.Semester)).Distinct().ToListAsync();
            return courseName;
        }
        /// <summary>
        /// 获取该课设名称有哪几个学期
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetTermForDesignName(string courseName)
        {
            var allTerm = await _courseEFRepository.GetAll().Where(c => c.Kind == "课设").Where(c => c.Name == courseName).Select(c => new SelectDto<string>(c.Semester, c.Semester)).Distinct().ToListAsync();
            return allTerm;
        }
        /// <summary>
        /// 根据课设名称和学期得到课程Id
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="semester"></param>
        /// <returns></returns>
        public async Task<Guid> GetDesignSelect(string courseName, string semester)
        {
            var course = await _courseEFRepository.FirstOrDefaultAsync(c =>c.Kind =="课设" && c.Name == courseName && c.Semester == semester);
            return course.Id;
        }
        /// <summary>
        /// 获取拥有该学期的课程
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<string>>> GetDesignNameForTrem(string semester)
        {
            var courseNames = await _courseEFRepository.GetAll().Where(c => c.Kind == "课设").Where(c => c.Semester == semester).Select(c => new SelectDto<string>(c.Name, c.Name)).Distinct().ToListAsync();
            return courseNames;
        }
        /// <summary>
        /// 获取课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CourseShowDto> GetCourse(Guid id)
        {
            var course = await _courseEFRepository.GetAsync(id);
            var show =  ObjectMapper.Map<CourseShowDto>(course);
            var classIds =  await _classCourseEFRepository.GetAll().Where(c=>c.CourseId == id).Select(c=>c.ClassId).ToListAsync();
            show.ClassIds = classIds;
            var outline = await _outlineHomeworkEFRepository.GetAsync(show.OutlineId);
            show.OutlineName = outline.Name;
            //题目个数
            var questions = await _questionEFRepository.GetAllListAsync(c => c.OutlineId == show.OutlineId);
            show.QuestionCount = questions.Count();
            //权重作业次数
            var scoreWeights = await _scoreweightEFRepository.GetAllListAsync(c=>c.OutlineId == show.OutlineId && c.Name != "期末考试");
            show.HomeWorkCount = scoreWeights.Select(c=>c.Times).Sum();
            return show;
        }
        /// <summary>
        /// 获取学生导入模板
        /// </summary>
        /// <returns></returns>
        public string GetImportStudentTemplate()
        {
            return _fileManagementAppService.PathToRelative(@"@\FileTemplate\学生导入模板.xlsx");
        }
        /// <summary>
        /// 从Excel中导入学生信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<ResultDto> ImportStudentFromExcel(string filePath)
        {
            var snos = await _studentEFRepository.GetAll().Select(c => c.Sno).ToListAsync();
            var classess = await _classesEFRepository.GetAllListAsync();
            var result = new ResultDto();
            //加载可读可写文件流
            filePath = _fileManagementAppService.PathToLocal(filePath);
            if (!File.Exists(filePath))
            {
                result.Result = false;
                result.Message = "文件不存在";
                return result;
            }
            FileStream fs = null;
            try
            {
                 fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                //var workbook = new XSSFWorkbook(fs);
                ISheet xssfSheet = null;
                if (Path.GetExtension(filePath) == ".xls")
                {
                    var workbook = new HSSFWorkbook(fs);
                    xssfSheet = workbook.GetSheetAt(0);
                }
                else if (Path.GetExtension(filePath) == ".xlsx")
                {
                    var workbook = new XSSFWorkbook(fs);
                    xssfSheet = workbook.GetSheetAt(0);
                }
                var positon = GetString(xssfSheet.GetRow(0)?.GetCell(0));
                if(positon != "班级")
                {
                    return new ResultDto(false, "表格数据与预期不符");
                }
                //第6行开始
                for (int rowIndex = 2; rowIndex <= xssfSheet.LastRowNum; rowIndex++)
                {
                    var student = new Student();
                    var xssfRow = xssfSheet.GetRow(rowIndex);
                    var classes = GetString(xssfRow.GetCell(0));
                    if(string.IsNullOrEmpty(classes))
                    {
                        return new ResultDto(true,"导入完成");
                    }
                    var cla = classess.FirstOrDefault(c => (c.SchoolYear + c.Major + c.Name) == classes);//找到与表中对应的班级
                    if (cla == null)
                    {
                        result = new ResultDto(false, "你导入的学生班级不存在，请先创建");
                        return result;
                    }
                    student.ClassId = cla.Id;
                    var sno = GetString(xssfRow.GetCell(1));
                    if (snos.Contains(sno))//如果学号已存在系统中则跳过
                    {
                        continue;
                    }
                    student.Sno = sno;
                    student.Name = GetString(xssfRow.GetCell(2));
                    student.Gender = GetString(xssfRow.GetCell(3)) == "男" ? true : false;
                    await _userAppService.PersonalRegister(new Users.Dto.RegisterDto
                    {
                        Name = student.Name,
                        Phone = sno,
                        ClassId = student.ClassId,
                        email = sno,
                        Role = "学生",
                        Gender = student.Gender,
                        Password = sno
                    });
                }
                return new ResultDto(true,"导入完成");
            }
            catch (Exception ex)
            {
                return new ResultDto(false,"发生异常");
            }
            finally
            {
                fs.Close();
            }
        }
        private string GetString(ICell xssfCell)
        {
            if (xssfCell == null)
            {
                return string.Empty;
            }
            if (xssfCell.CellType == CellType.Numeric || xssfCell.CellType == CellType.Formula)
            {
                //return Math.Round(xssfCell.NumericCellValue, MidpointRounding.AwayFromZero).ToString();
                return xssfCell.NumericCellValue.ToString();
            }
            else if (xssfCell.CellType == CellType.Boolean)
            {
                return xssfCell.BooleanCellValue.ToString();
            }
            else
            {
                return xssfCell.StringCellValue;
            }
        }
    }
}
