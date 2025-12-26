using Abp.Authorization;
using Abp.Domain.Repositories;
using Aspose.Words.Drawing.Charts;
using EduAdmin.AppService.CourseObjectives.Dto;
using EduAdmin.AppService.Homeworks;
using EduAdmin.AppService.Outlines.Dto;
using EduAdmin.AppService.ScoreWeights;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.FileManagements;
using EduAdmin.LocalTools;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using Abp.Linq.Extensions;
using Spire.Xls;
using EduAdmin.AppService.DefenseRecord.Dto;
using Abp.Domain.Uow;

namespace EduAdmin.AppService.Outlines
{
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class OutlineAppService : EduAdminAppServiceBase, IOutlineAppService
    {
        private readonly IRepository<Outline, Guid> _outlineEFRepository;
        private readonly IRepository<ScoreWeight, Guid> _scoreWeightEFRepository;
        private readonly IRepository<CourseObjective, Guid> _courseObjectiveEFRepository;
        private readonly IRepository<TestQuestion, Guid> _testQuestionEFRepository;
        private readonly IRepository<Question, Guid> _questionEFRepository;
        private readonly IRepository<Homework, Guid> _homeworkEFRepository;
        private readonly IRepository<Course, Guid> _courseEFRepository;
        private readonly IFileManagementAppService _fileManagementAppService;
        private readonly IHomeworkAppService _homeworkAppService;
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IRepository<StudentCourse, Guid> _studentCourseEFRepository;
        private readonly IRepository<StudentHomework, Guid> _studentHomeworkEFRepository;
        private readonly IRepository<QuestionScore, Guid> _questionScoreEFRepository;
        private readonly IRepository<Classes, Guid> _classesEFRepository;
        private readonly IRepository<ClassCourse, Guid> _classCourseEFRepository;
        private readonly IRepository<SwDetail, Guid> _swDetailEFRepository;

        private readonly IScoreWeightAppService _scoreWeightAppService;
        private readonly IHostingEnvironment _env;

        public OutlineAppService(IRepository<Outline, Guid> outlineEFRepository,
            IRepository<ScoreWeight, Guid> scoreWeightEFRepository,
            IRepository<CourseObjective, Guid> courseObjectiveEFRepository,
            IRepository<TestQuestion, Guid> testQuestionEFRepository,
            IRepository<Question, Guid> questionEFRepository,
            IRepository<Homework, Guid> homeworkEFRepository,
            IRepository<Course, Guid> courseEFRepository,
            IFileManagementAppService fileManagementAppService,
            IHomeworkAppService homeworkAppService,
            IRepository<Student, Guid> studentEFRepository,
            IRepository<StudentCourse, Guid> studentCourseEFRepository,
            IRepository<StudentHomework, Guid> studentHomeworkEFRepository,
            IRepository<QuestionScore, Guid> questionScoreEFRepository,
            IRepository<Classes, Guid> classesEFRepository,
            IRepository<ClassCourse, Guid> classCourseEFRepository,
            IScoreWeightAppService scoreWeightAppService,
            IRepository<SwDetail, Guid> swDetailEFRepository,
            IHostingEnvironment env)
        {
            _scoreWeightAppService = scoreWeightAppService;
            _outlineEFRepository = outlineEFRepository;
            _scoreWeightEFRepository = scoreWeightEFRepository;
            _courseObjectiveEFRepository = courseObjectiveEFRepository;
            _testQuestionEFRepository = testQuestionEFRepository;
            _questionEFRepository = questionEFRepository;
            _homeworkEFRepository = homeworkEFRepository;
            _courseEFRepository = courseEFRepository;
            _fileManagementAppService = fileManagementAppService;
            _homeworkAppService = homeworkAppService;
            _studentEFRepository = studentEFRepository;
            _studentCourseEFRepository = studentCourseEFRepository;
            _studentHomeworkEFRepository = studentHomeworkEFRepository;
            _questionScoreEFRepository = questionScoreEFRepository;
            _classCourseEFRepository = classCourseEFRepository;
            _classesEFRepository = classesEFRepository;
            _swDetailEFRepository = swDetailEFRepository;
            _env = env;
        }
        /// <summary>
        /// 获取所有课程大纲
        /// </summary>
        /// <returns></returns>
        public async Task<List<OutlineShowDto>> GetAllCourseOutline(Guid teacherId)
        {
            var outlineList = await _outlineEFRepository.GetAllListAsync(c => c.TeacherId == teacherId && c.Kind == "课程");
            return ObjectMapper.Map<List<OutlineShowDto>>(outlineList);
        }
        /// <summary>
        /// 获取所有课程大纲下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectDto<Guid>>> GetAllCourseOutlineSelect(Guid teacherId)
        {
            var outlineList = await _outlineEFRepository.GetAllListAsync(c => c.TeacherId == teacherId && c.IsComplete == true && c.Kind == "课程");
            return outlineList.Select(c => new SelectDto<Guid>(c.Id, c.Name)).ToList();
        }
        /// <summary>
        /// 获取所有课设大纲
        /// </summary>
        /// <returns></returns>
        public async Task<List<OutlineShowDto>> GetAllDesignOutline(Guid teacherId)
        {
            var outlineList = await _outlineEFRepository.GetAllListAsync(c => c.TeacherId == teacherId && c.Kind == "课设");
            return ObjectMapper.Map<List<OutlineShowDto>>(outlineList);
        }
        /// <summary>
        /// 获取所有课设大纲下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectDto<Guid>>> GetAllDesignSelect(Guid teacherId)
        {
            var outlineList = await _outlineEFRepository.GetAllListAsync(c => c.TeacherId == teacherId && c.IsComplete == true && c.Kind == "课设");
            return outlineList.Select(c => new SelectDto<Guid>(c.Id, c.Name)).ToList();
        }
        /// <summary>
        /// 删除不完整的大纲
        /// </summary>
        /// <param name="teacherId"></param>
        public async Task<bool> DeletedOutline(Guid teacherId)
        {
            var outline = await _outlineEFRepository.GetAllListAsync(c => c.TeacherId == teacherId && c.IsComplete == false);
            foreach (var item in outline)
            {
                try
                {
                        await DeleteOutline(item.Id);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> DeleteIsDeleteOutline()
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var outline = await _outlineEFRepository.GetAllListAsync(c => c.IsDeleted == true);
                foreach (var item in outline)
                {
                    try
                    {

                        await TrueDeleteOutline(item.Id);

                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// 添加大纲
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddOutline(OutlineCreateDto input)
        {
            var Outline = ObjectMapper.Map<Outline>(input);
            Outline.IsComplete = false;
            var id = await _outlineEFRepository.InsertAndGetIdAsync(Outline);
            if (input.Kind == "课程")
            {
                await _scoreWeightEFRepository.InsertAndGetIdAsync(new ScoreWeight
                {
                    Name = "期末考试",
                    Power = 70,
                    OutlineId = id,
                    Times = 0
                });
            }
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改大纲
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateOutline(OutlineCreateDto input)
        {
            var Outline = ObjectMapper.Map<Outline>(input);
            await _outlineEFRepository.UpdateAsync(Outline);
            return new UpdateResult();
        }
        /// <summary>
        /// 完全删除大纲
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<DeleteResult> TrueDeleteOutline(Guid id)
        {
            var bindOutline = await _courseEFRepository.GetAll().Select(c => c.OutlineId).ToListAsync();
            if (bindOutline.Contains(id))
            {
                return new DeleteResult("该大纲已被课程绑定，无法删除");
            }
            //删除相关成绩权重
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == id);
            foreach (var scoreWeight in scoreWeights)
            {
                await _scoreWeightAppService.DeleteScoreWeight(scoreWeight.Id);
            }
            //删除相关课程目标
            await _courseObjectiveEFRepository.DeleteAsync(c => c.OutlineId == id);
            //删除相关大题
            await _testQuestionEFRepository.DeleteAsync(c => c.OutlineId == id);
            //删除相关小题
            await _questionEFRepository.DeleteAsync(c => c.OutlineId == id);
            //删除大纲
            await _outlineEFRepository.HardDeleteAsync(c=>c.Id == id);
            await CurrentUnitOfWork.SaveChangesAsync();
            return new DeleteResult();
        }
        /// <summary>
        /// 删除大纲
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteOutline(Guid id)
        {
            var bindOutline = await _courseEFRepository.GetAll().Select(c => c.OutlineId).ToListAsync();
            if (bindOutline.Contains(id))
            {
                return new DeleteResult("该大纲已被课程绑定，无法删除");
            }
            //删除相关成绩权重
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == id);
            foreach (var scoreWeight in scoreWeights)
            {
                await _scoreWeightAppService.DeleteScoreWeight(scoreWeight.Id);
            }
            //删除相关课程目标
            await _courseObjectiveEFRepository.DeleteAsync(c => c.OutlineId == id);
            //删除相关大题
            await _testQuestionEFRepository.DeleteAsync(c => c.OutlineId == id);
            //删除相关小题
            await _questionEFRepository.DeleteAsync(c => c.OutlineId == id);
            //删除大纲
            await _outlineEFRepository.DeleteAsync(id);
            await CurrentUnitOfWork.SaveChangesAsync();
            return new DeleteResult();
        }
        /// <summary>
        /// 获取大纲
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OutlineShowDto> GetOutline(Guid id)
        {
            var Outline = await _outlineEFRepository.GetAsync(id);
            return ObjectMapper.Map<OutlineShowDto>(Outline);
        }
        /// <summary>
        /// 验证大纲是否完整
        /// </summary>
        /// <param name="outLineId"></param>
        /// <returns></returns>
        public async Task<ResultDto> CheckOutline(Guid outLineId)
        {
            var outLine = await _outlineEFRepository.GetAsync(outLineId);
            if (outLine.Name == null)
            {
                return new ResultDto(false, "请填写大纲名称");
            }
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outLineId);
            var courseObjectives = await _courseObjectiveEFRepository.GetAllListAsync(c => c.OutlineId == outLineId);
            var testQuestions = await _testQuestionEFRepository.GetAllListAsync(c => c.OutlineId == outLineId);
            var questions = await _questionEFRepository.GetAllListAsync(c => c.OutlineId == outLineId);
            var swDetails = await _swDetailEFRepository.GetAllListAsync(c => c.OutlineId == outLineId);
            var sDComplete = swDetails.FirstOrDefault(c => !c.Name.Contains("期末考试") && c.CourseObjectiveId == null);
            var sWComplete = scoreWeights.FirstOrDefault(c => c.Name == null || c.Power == null || c.Times == null);
            if (sDComplete != null)
            {
                outLine.IsComplete = false;
                return new ResultDto(false, "请将权重配置填写完整");
            }
            if (sWComplete != null)
            {
                outLine.IsComplete = false;
                return new ResultDto(false, "请将权重对应的课程目标填写完整");
            }
            if (scoreWeights.Count == 0 || courseObjectives.Count == 0 || testQuestions.Count == 0)
            {
                outLine.IsComplete = false;
                return new ResultDto(false, "请将信息填写完整");
            }
            var scoreWeightSum = scoreWeights.Select(c => c.Power).Sum();
            if (scoreWeightSum != 100)
            {
                outLine.IsComplete = false;
                return new ResultDto(false, "成绩权重之和不等于100%");
            }
            var courseObjectiveScores = courseObjectives.Select(c => c.ScoreProportion).ToList();
            var question = questions.Select(c => c.Score).Sum();
            var quesCourseObj = questions.FirstOrDefault(c => c.CourseObjectiveId == null);
            if (quesCourseObj != null)
            {
                outLine.IsComplete = false;
                return new ResultDto(false, "请将所有小题绑定课程目标");
            }
            if (question != 100)
            {
                outLine.IsComplete = false;
                return new ResultDto(false, "小题分数之和不等于100");
            }
            var testQuestionSum = testQuestions.Select(c => c.Score).Sum();
            if (testQuestionSum != 100)
            {
                outLine.IsComplete = false;
                return new ResultDto(false, "大题分数之和不等于100");
            }
            //大纲完整
            outLine.IsComplete = true;
            return new ResultDto(true, "大纲通过完整性验证");
        }
        /// <summary>
        /// 获取课程目标
        /// </summary>
        /// <param name="outlineId"></param>
        /// <returns></returns>
        //public async Task<bool> GetCourseObjForExam(Guid outlineId)
        //{
        //    List<SelectDto<Guid>> list = new List<SelectDto<Guid>>();
        //    var questions = await _questionEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
        //    var courseObjIds = questions.Select(c => c.CourseObjectivesId).ToList();
        //    var examWeight = await _scoreWeightEFRepository.FirstOrDefaultAsync(c => c.Name == "期末考试");
        //    var power = examWeight.Power;
        //    foreach (var courseObjId in courseObjIds)
        //    {
        //        int num = 0;
        //        foreach (var item in questions)
        //        {
        //            if (item.CourseObjectivesId == courseObjId)
        //            {
        //                num = (int)(num + item.Score);
        //            }
        //        }
        //        var couObj = await _courseObjectiveEFRepository.GetAsync(courseObjId);
        //        var scoreObj = JsonConvert.DeserializeObject<List<ScoreProportion>>(couObj.ScoreProportion);
        //        var scoreWeight = scoreObj.FirstOrDefault(c => c.Name == "期末考试");
        //        scoreWeight.Power = (float)(num * power);
        //        scoreObj.Remove(scoreWeight);
        //        scoreObj.Add(scoreWeight);
        //        var scoreJson = JsonConvert.SerializeObject(scoreObj);
        //        couObj.ScoreProportion = scoreJson;
        //    }
        //    return true;
        //}
        /// <summary>
        /// 导出需要导入成绩的表头
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<string> ExportExcelHead(Guid courseId)
        {
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            List<string> list = new List<string>();
            list.Add("班级");
            list.Add("学号");
            list.Add("姓名");
            list.Add("性别");
            //当前课堂所有作业
            var homeworks = await _homeworkEFRepository.GetAll().Where(c => c.CourseId == courseId).ToListAsync();
            //所有作业类型
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId && c.Name != "期末考试");
            var questions = await _questionEFRepository.GetAll().Where(c => c.OutlineId == outlineId).Select(c => c.TitleNum).ToListAsync();
            //把没有发布的作业放入表头
            foreach (var scoreWeight in scoreWeights)
            {
                var times = homeworks.Where(c => c.Type == scoreWeight.Id.ToString()).Select(c => c.Times).Distinct().ToList();
                for (int i = 1; i <= scoreWeight.Times; i++)
                {
                    if (!times.Contains(i))
                    {
                        list.Add(scoreWeight.Name + i);
                    }
                }
            }
            //把纸质的作业放入表头
            foreach (var scoreWeight in scoreWeights)
            {
                var times = homeworks.Where(c => c.Type == scoreWeight.Id.ToString() && c.FileType == "纸质").Select(c=>c.Times).Distinct().ToList();
                for (int i = 1; i <= scoreWeight.Times; i++)
                {
                    if (times.Contains(i))
                    {
                        list.Add(scoreWeight.Name + i);
                    }
                }
            }
            //找到大纲中的所有小题
            foreach (var question in questions)
            {
                list.Add(question.ToString());
            }
            var savePath = Path.Combine(_env.WebRootPath, "ExportExcels");
            var url = ExcelHelper.ExportExcel(list, savePath, "表头" + Guid.NewGuid());
            return url.Replace(_env.WebRootPath, LocalTool.GetAppSettings("path"));
        }
        /// <summary>
        /// 为导入的表头创建作业或小题
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<ResultDto> CompletionAllScore(Guid courseId)
        {
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            List<string> list = new List<string>();
            list.Add("班级");
            list.Add("学号");
            list.Add("姓名");
            list.Add("性别");
            //当前课堂所有作业
            var homeworks = await _homeworkEFRepository.GetAll().Where(c => c.CourseId == courseId).ToListAsync();
            //所有作业类型
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId && c.Name != "期末考试");
            var questions = await _questionEFRepository.GetAll().Where(c => c.OutlineId == outlineId).Select(c => c.TitleNum).ToListAsync();
            //把没有发布的作业放入表头
            foreach (var scoreWeight in scoreWeights)
            {
                var times = homeworks.Where(c => c.Type == scoreWeight.Id.ToString()).Select(c => c.Times).Distinct().ToList();
                for (int i = 1; i <= scoreWeight.Times; i++)
                {
                    if (!times.Contains(i))
                    {
                        list.Add(scoreWeight.Name + i);
                        await _homeworkAppService.AddHomework(new CreateHomeworkDto
                        {
                            Name = scoreWeight.Name + i,
                            FileType = "纸质",
                            CourseId = courseId,
                            Times = i,
                            Type = scoreWeight.Id.ToString(),
                            IsNotSendMessage = true
                        });
                    }
                }
            }
            //找到大纲中的所有小题
            foreach (var question in questions)
            {
                list.Add(question.ToString());
            }
            return new ResultDto();
        }
        /// <summary>
        /// 总表导出
        /// </summary>
        /// <returns></returns>
        public async Task<ResultDto> ExportAllTable(Guid courseId, Guid? classId)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var allTable = await GetAllTable(courseId, classId);
            var dicts = allTable.DicsList;
            if (dicts.Count == 0) return new ResultDto(false, "没有数据");
            foreach (var dict in dicts[0])
            {
                foreach (var item in allTable.List)
                {
                    if (dict.Key == item.Value)
                    {
                        keyValuePairs.Add(item.Label, dict.Value);
                    }
                }
            }
            var savePath = Path.Combine(_env.WebRootPath, "ExportExcels");
            var url = ExcelHelper.CreateExcel(allTable.DicsList, keyValuePairs, savePath, "总表" + Guid.NewGuid());
            var nurl = _fileManagementAppService.PathToRelative(url);
            //var pdfPath = Path.Combine(_env.WebRootPath, "PDFs", "总表导出");
            //if (!Directory.Exists(pdfPath))
            //{
            //    Directory.CreateDirectory(pdfPath);
            //}
            //var pdf = OfficeConvert.GetFilePdf(nurl, pdfPath, _env.WebRootPath, LocalTool.GetAppSettings("path"));
            return new ResultDto(true, nurl);
        }
        /// <summary>
        /// 获取总表
        /// </summary>
        /// <returns></returns>
        public async Task<AllTableDto> GetAllTable(Guid courseId, Guid? classId)
        {
            List<Dictionary<string, string>> dicsList = new List<Dictionary<string, string>>();
            List<SelectDto<string>> list = new List<SelectDto<string>>();
            #region 数据库查询部分
            //课程的大纲
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            //班级所有学生
            var classIds = await _classCourseEFRepository.GetAll().Where(c => c.CourseId == courseId).Select(c => c.ClassId).ToListAsync();
            var students = await _studentEFRepository.GetAllListAsync(c => classIds.Contains(c.ClassId));
            if (classId != null)
            {
                students = students.Where(c => c.ClassId == classId).ToList();
            }
            //该班级课程所有作业
            var homeworks = await _homeworkEFRepository.GetAllListAsync(c => c.CourseId == courseId && classIds.Contains(c.ClassesId));
            if (classId != null)
            {
                homeworks = homeworks.Where(c => c.ClassesId == classId).ToList();
            }
            //所有作业分数
            var stuHomeworks = await _studentHomeworkEFRepository.GetAllListAsync();
            //所有作业类型
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId && c.Name != "期末考试");
            //期末考试
            var exam = await _scoreWeightEFRepository.FirstOrDefaultAsync(c => c.OutlineId == outlineId && c.Name == "期末考试");
            //所有小题及分数
            var questionScores = await _questionScoreEFRepository.GetAllListAsync(c => c.CourseId == courseId);
            var questions = await _questionEFRepository.GetAll().Where(c => c.OutlineId == outlineId).OrderBy(c => c.TitleNum).ToListAsync();
            //所有班级
            var classes = await _classesEFRepository.GetAllListAsync();
            #endregion
            #region 表头部分
            list.Add(new SelectDto<string>("class", "班级"));
            list.Add(new SelectDto<string>("sno", "学号"));
            list.Add(new SelectDto<string>("stuName", "姓名"));
            list.Add(new SelectDto<string>("gender", "性别"));
            //作业表头部分
            char flagHead = 'a';
            for (var i = 0; i < scoreWeights.Count(); i++)
            {
                for (var j = 1; j <= scoreWeights[i].Times; j++)
                {
                    list.Add(new SelectDto<string>("sc" + flagHead + j, scoreWeights[i].Name + j));
                }
                flagHead++;
            }
            list.Add(new SelectDto<string>("usualScore", "平时成绩"));
            //小题表头部分
            for (int i = 1; i <= questions.Count; i++)
            {
                bool isContainNum = false;
                for (int j = 0; j < questions[i - 1].TitleNum.Length; j++)
                {
                    if (Char.IsNumber(questions[i - 1].TitleNum, j))
                    {
                        isContainNum = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (isContainNum == true)
                    list.Add(new SelectDto<string>("test" + i, "小题" + questions[i - 1].TitleNum));
                else
                {
                    list.Add(new SelectDto<string>("test" + i, questions[i - 1].TitleNum));
                }
            }
            list.Add(new SelectDto<string>("allScore", "总评"));
            #endregion
            foreach (var student in students)
            {
                Dictionary<string, string> dics = new Dictionary<string, string>();
                //学生的所有小题
                var stuQuesScores = questionScores.Where(c => c.StudentId == student.Id).OrderBy(c => c.TitleNum).ToList();
                var cla = classes.FirstOrDefault(c => c.Id == student.ClassId);
                dics.Add("class", cla.SchoolYear + cla.Major + cla.Name);
                dics.Add("sno", student.Sno);
                dics.Add("stuName", student.Name);
                dics.Add("gender", student.Gender == true ? "男" : "女");
                //作业数据部分
                float usualScore = 0;
                char flag = 'a';
                for (var i = 0; i < scoreWeights.Count(); i++)
                {
                    var scoreWeight = scoreWeights[i];
                    var homeworkList = homeworks.Where(c => c.Type == (scoreWeights[i].Id).ToString()).ToList();
                    if (homeworkList.Count == 0)
                    {
                        for (var j = 1; j <= scoreWeights[i].Times; j++)
                        {
                            dics.Add("sc" + flag + j, null);
                        }
                    }
                    else
                    {
                        for (var j = 1; j <= scoreWeights[i].Times; j++)
                        {
                            var homework = homeworkList.FirstOrDefault(c => c.Times == j);
                            if (homework != null)
                            {
                                var stuHomework = stuHomeworks.FirstOrDefault(c => c.StudentId == student.Id && c.HomeworkId == homework.Id);
                                if (stuHomework != null)
                                {
                                    dics.Add("sc" + flag + j, stuHomework.Score.ToString());
                                    int stuScore = 0;
                                    if (stuHomework.Score != null)
                                        stuScore = (int)stuHomework.Score;
                                    usualScore = (float)(usualScore + scoreWeight.Power / 100 / scoreWeight.Times * stuScore);
                                }
                                else
                                    dics.Add("sc" + flag + j, null);
                            }
                        }
                    }
                    flag++;
                }
                //平时成绩
                var realUsualScore = (float)Math.Round((decimal)(usualScore / (1 - exam.Power / 100)), 2);
                dics.Add("usualScore", realUsualScore.ToString());
                //小题数据部分
                float allScore = 0;
                for (int i = 1; i <= questions.Count; i++)
                {
                    var stuScore = stuQuesScores.FirstOrDefault(c => c.TitleNum == questions[i - 1].TitleNum);
                    dics.Add("test" + i, stuScore?.Score.ToString());
                    if (stuScore != null)
                        allScore = (float)(allScore + stuScore?.Score);
                }
                var realAllScore = Math.Round((decimal)(realUsualScore * (1 - exam.Power / 100) + allScore * exam.Power / 100), 2);
                dics.Add("allScore", realAllScore.ToString());
                dicsList.Add(dics);
            }
            //冒泡排序
            for (int i = 0; i < dicsList.Count() - 1; i++)
            {
                for (int j = 0; j < dicsList.Count() - 1 - i; j++)
                {
                    var allSc = dicsList[j].FirstOrDefault(c => c.Key == "allScore");
                    var allScN = dicsList[j + 1].FirstOrDefault(c => c.Key == "allScore");
                    var ScInt = float.Parse(allSc.Value);
                    var ScNInt = float.Parse(allScN.Value);
                    if (ScNInt > ScInt)
                    {
                        var tem = dicsList[j];
                        dicsList[j] = dicsList[j + 1];
                        dicsList[j + 1] = tem;
                    }
                }
            }
            return new AllTableDto
            {
                DicsList = dicsList,
                List = list
            };
        }
        /// <summary>
        /// 导入所有成绩
        /// </summary>
        public async Task<ResultDto> ImportScoreExcel(string filePath, Guid courseId)
        {
            //创建所有作业
            await CompletionAllScore(courseId);
            #region 数据查找部分
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            //作业类型名称列表
            var scoreweightAll = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            var scoreWeights = scoreweightAll.Select(c => c.Name).ToList();
            var studentIds = await _studentCourseEFRepository.GetAll().Where(c => c.CourseId == courseId).Select(c => c.StudentId).ToListAsync();
            //拥有该课程的学生
            var students = await _studentEFRepository.GetAllListAsync(c => studentIds.Contains(c.Id));
            //属于该课程的作业
            var homeworks = await _homeworkEFRepository.GetAllListAsync(c => c.CourseId == courseId);
            //所有学生作业
            var stuHomeworks = await _studentHomeworkEFRepository.GetAllListAsync();
            //学生考试小题成绩
            var questionScores = await _questionScoreEFRepository.GetAllListAsync(c => c.CourseId == courseId);
            #endregion
            List<string> list1 = new List<string>();//用于顺序存储作业类型表头
            List<string> list2 = new List<string>();//用于顺序存储小题表头
            var result = new ResultDto(true, "导入成功");
            //加载可读可写文件流
            filePath = _fileManagementAppService.PathToLocal(filePath);
            if (!File.Exists(filePath))
            {
                result.Result = false;
                result.Message = "文件不存在";
                return result;
            }
            try
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
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
                int examIndex = 0;
                //复杂循环取值导入
                //匹配作业类型对应表头
                for (int columnIndex = 4; columnIndex > 0; columnIndex++)
                {
                    var workName = GetString(xssfSheet.GetRow(0).GetCell(columnIndex));
                    var workNameReal = Regex.Replace(workName, @"\d", "");
                    if (scoreWeights.Contains(workNameReal))
                    {
                        list1.Add(workName);
                    }
                    else
                    {
                        examIndex = columnIndex;
                        break;
                    }
                }
                //throw new Exception();
                var changeValue = examIndex;//作业结束后考试成绩开始读取的列
                //考试小题对应表头
                for (int columnIndex = examIndex; columnIndex > 0; columnIndex++)
                {
                    var workName = GetString(xssfSheet.GetRow(0).GetCell(columnIndex));
                    if (string.IsNullOrEmpty(workName))
                    {
                        break;
                    }
                    if (!string.IsNullOrEmpty(workName))
                    {
                        list2.Add(workName);
                    }
                }
                //从第二行开始
                for (int rowIndex = 1; rowIndex <= xssfSheet.LastRowNum; rowIndex++)
                {
                    var xssfRow = xssfSheet.GetRow(rowIndex);
                    var sno = GetString(xssfRow.GetCell(1));
                    var student = students.FirstOrDefault(c => c.Sno == sno);
                    if(student == null)
                        return new ResultDto(false, "你导入的学生不存在");
                    var qScores = questionScores.Where(c => c.StudentId == student.Id).ToList();
                    //作业部分
                    for (var column = 4; column < changeValue; column++)
                    {
                        //var homework = homeworks.FirstOrDefault(c => c.Name+c.Times == list1[column - 4]);//作业
                        Homework homework = null;
                        homeworks.ForEach(c =>
                        {
                            var sw = (scoreweightAll.FirstOrDefault(s => s.Id.ToString() == c.Type)).Name;//该作业对应的权重名称
                            if (sw + c.Times == list1[column - 4] && c.ClassesId == student.ClassId)
                                homework = c;

                        });
                        if (homework == null)
                        {
                            return new ResultDto(false, "作业为空");
                        }
                        var score = GetString(xssfRow.GetCell(column));
                        var stuHomework = stuHomeworks.FirstOrDefault(c => c.StudentId == student.Id && c.HomeworkId == homework.Id);
                        stuHomework.Remark = null;
                        stuHomework.Score = double.Parse(score);
                        stuHomework.State = true;
                        await _studentHomeworkEFRepository.UpdateAsync(stuHomework);
                    }
                    //考试题目部分
                    for (var column = changeValue; column < changeValue + list2.Count(); column++)
                    {
                        var titleName = list2[column - changeValue];
                        var score = double.Parse(GetString(xssfRow.GetCell(column)));
                        var qScore = qScores.FirstOrDefault(c => c.TitleNum == titleName);
                        if (qScore == null)
                        {
                            await _questionScoreEFRepository.InsertAsync(new QuestionScore
                            {
                                StudentId = student.Id,
                                TitleNum = titleName,
                                Score = score,
                                CourseId = courseId
                            });
                        }
                        else
                        {
                            qScore.Score = score;
                            await _questionScoreEFRepository.UpdateAsync(qScore);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return new ResultDto(false, "导入失败");
            }
        }
        /// <summary>
        /// 生成目标达成度统计图标
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<List<string>> ExportChart(Guid courseId, Guid? classId)
        {
            var clas = await _classesEFRepository.GetAllListAsync(); 
            var students = await _studentEFRepository.GetAllListAsync();
            var allCompute = await ComputeDegreeAchieve(courseId, classId);
            var computeAlone = allCompute.ComputeAlone;
            var compute = allCompute.CourseObjAchives;
            var cObjList = compute.Select(c => c.StudentId).Distinct().ToList();
            var cObjs = compute.Select(c => c.CourseObjectiveName).Distinct().ToList();
            List<Dictionary<string, string>> dictList = new List<Dictionary<string, string>>();
            foreach(var cObj in cObjList)
            {
                var thisCompute = computeAlone.FirstOrDefault(c => c.Contains(new KeyValuePair<string, string>("studentId", cObj.ToString())));
                thisCompute.Remove("studentId");
                Dictionary<string, string> dics = new Dictionary<string, string>();
                var stuAchive = compute.Where(c => c.StudentId == cObj).ToList();
                var stu = students.FirstOrDefault(c=>c.Id == cObj);
                dics.Add("姓名", stu.Name);
                foreach(var item in thisCompute)
                {
                    dics.Add(item.Key, item.Value);
                }
                foreach (var item in stuAchive)
                {
                    dics.Add(item.CourseObjectiveName, item.Score.ToString());
                }
                dictList.Add(dics);
            }
            Dictionary<string, string> dics1 = new Dictionary<string, string>();
            foreach (var item in cObjs)
            {
                var all = compute.Where(c=>c.CourseObjectiveName == item).Select(c => c.Score);
                var sum = all.Sum();
                var count = all.Count();
                var avg = sum / count;
                dics1.Add(item, avg.ToString());
            }
            dictList.Add(dics1);
            var savePathExcel = Path.Combine(_env.WebRootPath, "ExportExcels");
            var filePath = ExcelHelper.CreateExcelToint(dictList, dictList[0], savePathExcel, "表头" + Guid.NewGuid());
            //加载可读可写文件流
            var result = new ResultDto(true, "导入成功");
            filePath = _fileManagementAppService.PathToLocal(filePath);
            if (!File.Exists(filePath))
            {
                result.Result = false;
                result.Message = "文件不存在";
                return null;
            }
            var dN = Path.GetDirectoryName(filePath);
            // 创建一个Workbook类实例，加载Excel文档
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(filePath);
            //获取第一个工作表
            Worksheet sheet = workbook.Worksheets[0];

            if(classId != null)
            {
                var cla = clas.FirstOrDefault(c=>c.Id == classId);
                var claName = cla.SchoolYear + cla.Major + cla.Name;
                sheet.Name = claName + "目标达成度";
            }
            //设置工作表的名称
            sheet.Name = "专业目标达成度";
            sheet.GridLinesVisible = true;
            //表格列长度突破26后转换
            string colum = "";
            var columStr = 'B';
            char Ascll = 'A';
            for (int i = 0; i < computeAlone[0].Count(); i++)
            {
                if (columStr >= 'Z')
                {
                    colum = "A"+ Ascll;
                    Ascll++;
                }
                else
                    columStr++;
                colum = columStr.ToString();
            }
            var row = dictList.Count()-1;
            var n = 0;
            foreach(var item in cObjs)
            {
                Spire.Xls.Chart chart = sheet.Charts.Add(ExcelChartType.ScatterMarkers);
                var range = colum + "2" + ":" + colum + (1 + row);
                chart.DataRange = sheet.Range[range];
                chart.SeriesDataFromRange = false;
                chart.LeftColumn = 5 + computeAlone[0].Count();
                chart.TopRow = 3+n*13;
                chart.RightColumn = 15 + computeAlone[0].Count();
                chart.BottomRow = 15+n*13;
                chart.ChartTitle = "学生个体对"+ item + "达成情况分析";
                chart.ChartTitleArea.IsBold = true;
                chart.ChartTitleArea.Size = 12;
                //chart.Series.ClearDataFormats();
                chart.Series[0].CategoryLabels = sheet.Range["A2:A" + 1 + row];
                chart.Series[0].CategoryLabels.Style.NumberFormat = "#,##";

                //设置X轴坐标名称及字体格式
                //chart.PrimaryCategoryAxis.Title = "产品类别";
                //chart.PrimaryCategoryAxis.Font.IsBold = true;
                //chart.PrimaryCategoryAxis.TitleArea.IsBold = false;

                //设置Y轴坐标名称及字体格式
                //chart.PrimaryValueAxis.Title = "销售额";
                //chart.PrimaryValueAxis.HasMajorGridLines = false;
                //chart.PrimaryValueAxis.TitleArea.TextRotationAngle = 90;
                //chart.PrimaryValueAxis.MinValue = 0.5;
                //chart.PrimaryValueAxis.TitleArea.IsBold = false;
                //设置图例的位置
                chart.Legend.Position = LegendPositionType.Right;
                chart.HasLegend = false;
                colum = CharNum(colum, true);
                //chart.SaveToImage(Path.Combine(dN, Guid.NewGuid().ToString() + ".png"));
                n++;
            }
            colum = CharNum(colum, false);
            string columStar = colum;
            for (int i =0;i< cObjs.Count()-1; i++)
            {
                columStar = CharNum(columStar, false);
            }
            Spire.Xls.Chart chart1 = sheet.Charts.Add(ExcelChartType.ColumnClustered);
            chart1.DataRange = sheet.Range[columStar + (2 + row)+":"+ colum+ (2 + row)];
            chart1.SeriesDataFromRange = true;
            chart1.LeftColumn = 16+ computeAlone[0].Count();
            chart1.TopRow = 10;
            chart1.RightColumn = 22+ computeAlone[0].Count();
            chart1.BottomRow = 22;
            chart1.ChartTitle = "课程目标达成情况分析";
            chart1.ChartTitleArea.IsBold = true;
            chart1.ChartTitleArea.Size = 12;
            chart1.HasLegend = false;

            List<string> urls = new List<string>();
            var savePath = Path.Combine(dN, Guid.NewGuid().ToString() + ".xlsx");
            //保存文档
            workbook.SaveToFile(savePath, ExcelVersion.Version2016);

            //加载生成图表后的Excel文档
            workbook.LoadFromFile(savePath, ExcelVersion.Version2016);

            urls.Add(_fileManagementAppService.PathToRelative(savePath));
                //遍历工作簿，诊断是否包含图表
                Image[] images = workbook.SaveChartAsImage(sheet);

            for (int i = 0; i < images.Length; i++)
            {
                var imagePath = Path.Combine(dN, Guid.NewGuid().ToString() + i + ".png");
                //将图表保存为图片
                images[i].Save(imagePath, ImageFormat.Png);
                urls.Add(_fileManagementAppService.PathToRelative(imagePath));
            }
            //}
            //catch (Exception ex)
            //{
            //    return new ResultDto(false,"触发异常");
            //}
            return urls;
        }
        /// <summary>
        /// 列加一和减一
        /// </summary>
        /// <param name="colum"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string CharNum(string colum, bool flag)
        {
            var char0 = colum.ToCharArray();
            if (char0.Length == 1)
            {
                var index = char0[0];
                if (flag == false)
                    index--;
                else
                    index++;
                colum = index.ToString();
            }
            else
            {
                var index = char0[1];
                if (flag == false)
                    index--;
                else
                    index++;
                colum = char0[0].ToString() + index;
            }
            return colum;
        }
        /// <summary>
        /// 计算目标达成度
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<AllCompute> ComputeDegreeAchieve(Guid courseId, Guid? classId)
        {
            var classStu = await _classCourseEFRepository.GetAll().Where(c => c.CourseId == courseId).Select(c => c.ClassId).ToListAsync();
            //当前班级的所有学生
            var students = await _studentEFRepository.GetAll().Where(c=> classStu.Contains(c.ClassId)).WhereIf(classId != null,c => c.ClassId == classId).ToListAsync();
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            //所有作业类型
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId && c.Name != "期末考试");
            //期末考试
            var exam = await _scoreWeightEFRepository.FirstOrDefaultAsync(c => c.OutlineId == outlineId && c.Name == "期末考试");
            //所有课程目标
            var courseObjectives = await _courseObjectiveEFRepository.GetAllListAsync(c=>c.OutlineId == outlineId);
            //当前课程所有作业
            var homeworks = await _homeworkEFRepository.GetAllListAsync(c=>c.CourseId == courseId);
            var homeworkIds = homeworks.Select(c => c.Id).ToList();
            //当前课程所有学生作业
            var studentHomeworks = await _studentHomeworkEFRepository.GetAllListAsync(c=> homeworkIds.Contains(c.HomeworkId));
            //权重详细
            var swDetails = await _swDetailEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            //所有小题分值配置
            var question = await _questionEFRepository.GetAllListAsync(c=>c.OutlineId==outlineId);
            //所有小题得分
            var questionScore = await _questionScoreEFRepository.GetAllListAsync(c=>c.CourseId==courseId);
            //梳理大纲
            List<CourseObjAchive> courseObjCompute = new List<CourseObjAchive>();
            List<Dictionary<string,string>> dictList = new List<Dictionary<string,string>>();
            foreach (var student in students)
            {
                Dictionary<string, string> dicts = new Dictionary<string, string>();
                dicts.Add("studentId", student.Id.ToString());
                var studentQuesScore = questionScore.Where(c => c.StudentId == student.Id);
                //课程目标达成度计算公式：如果一个目标占一个权重类型的多个小点则按照比重获取均值
                //（如：课设1作业1作业2 (作业1+作业2)/2*ScoreWeight.power）
                //课程目标i的达成度 = E考核环节平均分*考核环节权重/（E考核环节应得分*考核环节权重）
                foreach (var item in courseObjectives)
                {
                    float molecule = 0;//分子
                    float denominator = 0;//分母
                    //平时成绩部分对应课程目标
                    foreach (var ite in scoreWeights)
                    {
                        var swDetail = swDetails.Where(c => c.CourseObjectiveId == item.Id && c.ScoreWeightId == ite.Id).Select(c=>c.Times).Distinct().OrderBy(c=>c).ToList();//所有题号
                        var hWorks = homeworks.Where(c => c.Type == ite.Id.ToString() && swDetail.Contains(c.Times)).ToList();//所有作业
                        var homeworkId = hWorks.Select(c=>c.Id).ToList();//所有作业ID
                        var stuHomework = studentHomeworks.Where(c=>c.StudentId == student.Id && homeworkId.Contains(c.HomeworkId)).ToList();//学生作业
                        if (stuHomework.Count() == 0) continue;
                        var thisM = (float)(stuHomework.Select(c => c.Score).Sum() / stuHomework.Count() * ite.Power / 100);//当前作业权重平均值
                        var thisD = (float)(100 * ite.Power / 100);
                        molecule = molecule + thisM;
                        denominator = denominator + thisD;

                        //计算单条达成度
                        var achiveAlone =Math.Round(thisM / thisD,2);
                        var name = item.Name + ite.Name;
                        foreach(var it in swDetail)
                        {
                            name = name + it +",";
                        }
                        name = name.Substring(0, name.Length - 1);
                        dicts.Add(name, achiveAlone.ToString());
                    }

                    //考试部分对应课程目标
                    var ques = question.Where(c => c.CourseObjectiveId == item.Id).ToList();
                    foreach (var que in ques)
                    {
                        var stuQScore = studentQuesScore.FirstOrDefault(c => c.TitleNum == que.TitleNum);
                        var thisM = stuQScore?.Score * exam.Power / 100;
                        if (thisM == null) thisM = 0;
                        var thisD = que.Score * exam.Power / 100;
                        molecule = (float)(molecule + thisM);
                        denominator = denominator + (float)thisD;

                        var achiveAlone = Math.Round((double)(thisM / thisD),2);
                        var name = item.Name + "考试" + que.TitleNum;
                        dicts.Add(name, achiveAlone.ToString());
                    }
                    dictList.Add(dicts);
                    courseObjCompute.Add(new CourseObjAchive
                    {
                        CourseObjectiveName = item.Name,
                        Score = (float)Math.Round(molecule / denominator,2),
                        StudentId = student.Id
                    });
                }
            }
            return new AllCompute
            {
                ComputeAlone = dictList,
                CourseObjAchives = courseObjCompute
            };
        }
        public async Task<List<CourObjCreateTableDto>> testTable(Guid courseId, Guid classId)
        {
          var compute = await ComputeDegreeAchieve(courseId, classId);
          var res =  await CourObjCreateTable(compute, courseId, classId);
            return res;
        }
        /// <summary>
        /// 专业目标达成度Table
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<List<CourObjCreateTableForMajor>> CourObjCreateTableForMajor(Guid courseId)
        {
            List<CourObjCreateTableForMajor> list = new List<CourObjCreateTableForMajor>();
            var claIds = await _classCourseEFRepository.GetAll().Where(c => c.CourseId == courseId).Select(c => c.ClassId).ToListAsync();//获取所有班级的Id
            var clas = await _classesEFRepository.GetAllListAsync();
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            var courseObjs = await _courseObjectiveEFRepository.GetAll().Where(c => c.OutlineId == outlineId).Select(c => new SelectDto<string>(c.Name, c.Content)).ToListAsync();//课程目标名称
            foreach (var item in courseObjs)
            {
                List<ClassAchiveDto> list1 = new List<ClassAchiveDto>();
                foreach (var cla in claIds)
                {
                    var compute = await ComputeDegreeAchieve(courseId, cla);
                    var courseObjAchives = compute.CourseObjAchives.Where(c => c.CourseObjectiveName == item.Value);
                    string achiveScore = "0";
                    if (courseObjAchives.Count() > 0)
                    {
                        achiveScore = Math.Round(courseObjAchives.Select(c => c.Score).Sum() / courseObjAchives.Count(), 2).ToString();
                    }
                    var className = clas.FirstOrDefault(c => c.Id == cla)?.Name;
                    list1.Add(new ClassAchiveDto
                    {
                        ClassName = className,
                        AchiveScore = achiveScore,
                    });
                }
                list.Add(new CourObjCreateTableForMajor
                {
                    CourObjContent = " " + item.Value + ":" + item.Label,
                    ClassAchive = list1.OrderBy(c=>c.ClassName).ToList(),
                });
            }
            return list;
        }
        /// <summary>
        /// 班级目标达成度Table
        /// </summary>
        /// <param name="compute"></param>
        /// <param name="courseId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<List<CourObjCreateTableDto>> CourObjCreateTable(AllCompute compute, Guid courseId, Guid classId)
        {
            List<CourObjCreateTableDto> list = new List<CourObjCreateTableDto>();
            var outlineId = (await _courseEFRepository.GetAsync(courseId)).OutlineId;
            var courseObjs = await _courseObjectiveEFRepository.GetAll().Where(c => c.OutlineId == outlineId).Select(c => new SelectDto<string>(c.Name, c.Content)).ToListAsync();//课程目标名称
            var students = await _studentEFRepository.GetAllListAsync(c => c.ClassId == classId);
            var scoreWeights = await _scoreWeightEFRepository.GetAll().Where(c => c.OutlineId == outlineId).ToListAsync();//当前课程所有权重
            var homeworks = await _homeworkEFRepository.GetAllListAsync(c => c.CourseId == courseId && c.ClassesId == classId);
            var homeworkIds = homeworks.Select(c => c.Id).ToList();
            var studentHomeworks = await _studentHomeworkEFRepository.GetAllListAsync(c=> homeworkIds.Contains(c.HomeworkId));
            var questions = await _questionEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            var questionScores = await _questionScoreEFRepository.GetAllListAsync(c => c.CourseId == courseId);
            var aloneCompute = compute.ComputeAlone;
            foreach (var courseObj in courseObjs)
            {
                var allAchive = compute.CourseObjAchives.Where(c => c.CourseObjectiveName == courseObj.Value).Select(c=>c.Score).Sum();
                var allAchiveScore = Math.Round(allAchive / students.Count(),2);

                List<TableSplitDto> list1 = new List<TableSplitDto>();
                var objAchives = aloneCompute[0].Where(c => c.Key.Contains(courseObj.Value));
                var count = objAchives.Count();
                foreach (var item in objAchives)
                {
                    string rate = null;
                    string objScore = null;
                    string scoreAve = null;
                    var rateLabel = item.Key.Replace(courseObj.Value, "");
                    if (!rateLabel.Contains("考试"))
                    {
                        var scoreWeight = scoreWeights.FirstOrDefault(c => rateLabel.Contains(c.Name));
                        rate = rateLabel + "(" + scoreWeight.Power + "%" + ")";
                        objScore = 100.ToString();
                        var str = rateLabel.Split(",");
                        double avg = 0;
                        foreach (var num in str)
                        {
                            var number = Regex.Replace(num, @"[^0-9]+", "");
                            var homework = homeworks.FirstOrDefault(c => c.Type == scoreWeight.Id.ToString() && c.Times.ToString() == number);
                            var allScore = studentHomeworks.Where(c => c.HomeworkId == homework?.Id).Select(c => c.Score).ToList();
                            if (allScore.Count() > 0)
                                avg += Math.Round((double)allScore.Sum() / allScore.Count(), 2);
                        }
                        scoreAve = Math.Round(avg / str.Length, 2).ToString();
                    }
                    else//如果是考试题目
                    {
                        var scoreweightExam = scoreWeights.FirstOrDefault(c => c.Name == "期末考试");
                        bool isContainNum = false;
                        for (int i = 0; i < rateLabel.Length; i++)
                        {
                            if (Char.IsNumber(rateLabel, i))
                            {
                                isContainNum = true;
                                break;
                                               }
                            else
                            {
                                continue;
                            }
                        }
                        string titleNum = null;
                        if (isContainNum == true)
                            titleNum = Regex.Replace(rateLabel, @"[^\d.\d]", "");
                        else
                            titleNum = rateLabel.Replace("考试", "");
                        rate = "期末考试"+titleNum+"("+ scoreweightExam.Power + "%" + ")";
                        objScore = questions.FirstOrDefault(c => c.TitleNum.ToString() == titleNum)?.Score.ToString();
                        var qs = questionScores.Where(c => c.TitleNum.ToString() == titleNum).Select(c => c.Score);
                        scoreAve = Math.Round((double)((double)qs.Sum()/qs.Count()),2).ToString();
                    }
                    double achiveScore = 0;
                    foreach(var ite in aloneCompute)
                    {
                        achiveScore += double.Parse(ite.FirstOrDefault(c => c.Key == item.Key).Value);
                    }
                    achiveScore = Math.Round(achiveScore/aloneCompute.Count(),2);

                    list1.Add(new TableSplitDto
                    {
                        Rate = rate,
                        ObjScore = objScore,
                        AvgScore = scoreAve,
                        AchiveScore = achiveScore.ToString()
                    });
                }
                list.Add(new CourObjCreateTableDto
                {
                    CourObjContent =" " + courseObj.Value + ":" + courseObj.Label,
                    TableSplit = list1,
                    AllAchiveScore = allAchiveScore.ToString(),
                    Count = count,
                });
            }
            return list;
        }
        /// <summary>
        /// 导出目标达成度word（数据量大）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<string>> ExportAchiveComputeWord(EcportAchiveInputDto input)
        {
            Dictionary<string,string> dicts = new Dictionary<string, string>();
            var course = await _courseEFRepository.FirstOrDefaultAsync(c => c.Id == input.CourseId);
            var claCourseIds = await _classCourseEFRepository.GetAll().Where(c => c.CourseId == input.CourseId).Select(c=>c.ClassId).ToListAsync();
            var students = await _studentEFRepository.GetAll().Where(c => claCourseIds.Contains(c.ClassId)).WhereIf(input.ClassId != null, c => c.ClassId == input.ClassId).ToListAsync();
            var clas = await _classesEFRepository.GetAll().Where(c=> claCourseIds.Contains(c.Id)).WhereIf(input.ClassId != null, c => c.Id == input.ClassId).ToListAsync();
            var outlineId = (await _courseEFRepository.GetAsync(input.CourseId)).OutlineId;
            var testQuestions = await _testQuestionEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            var questions = await _questionEFRepository.GetAllListAsync(c=>c.OutlineId == outlineId);
            //所有作业类型
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId && c.Name != "期末考试");
            //期末考试
            var exam = await _scoreWeightEFRepository.FirstOrDefaultAsync(c => c.OutlineId == outlineId && c.Name == "期末考试");
            var urls = await ExportChart(input.CourseId, input.ClassId);
            var excleTable = urls.FirstOrDefault();
            var analEvaluaChart = _fileManagementAppService.PathToLocal(urls.LastOrDefault());
            StringBuilder sb = new StringBuilder();
            for(var i = 1; i < urls.Count()-1; i++)
            {
                sb.Append(_fileManagementAppService.PathToLocal(urls[i]));
                sb.Append(",");
            }
            var studentAnalEvaluaChart = sb.ToString().Substring(0, sb.Length - 1);
            List<CourObjCreateTableForMajor> courObjCreateTableForMajor = null;
            List<CourObjCreateTableDto> courObjCreateTable = null;
            //表格部分
            if (input.ClassId == null)
            {
                 courObjCreateTableForMajor = await CourObjCreateTableForMajor(input.CourseId);
            }
            else
            {
                var compute = await ComputeDegreeAchieve(input.CourseId, input.ClassId);
                courObjCreateTable = await CourObjCreateTable(compute, input.CourseId, (Guid)input.ClassId);
            }
            dicts.Add("CourseObjectTable", "");
            //特殊字符
            if (input.EvaluationMethod == 1)
            {
                dicts.Add("1221A", "\u221A");
            }
            else if(input.EvaluationMethod == 2)
            {
                dicts.Add("2221A", "\u221A");
            }
            if (input.FillBill == 1)
            {
                dicts.Add("3221A", "\u221A");
            }
            else if (input.FillBill == 2)
            {
                dicts.Add("4221A", "\u221A");
            }else if(input.FillBill == 2)
            {
                dicts.Add("5221A", "\u221A");
            }
            if (input.TestDifficulty == 1)
            {
                dicts.Add("6221A", "\u221A");
            }
            else if (input.TestDifficulty == 2)
            {
                dicts.Add("7221A", "\u221A");
            }
            else if (input.TestDifficulty == 3)
            {
                dicts.Add("8221A", "\u221A");
            }
            //图表
            if(input.ClassId == null)
            {
                dicts.Add("Title", "按专业");
            }
            else
            {
                dicts.Add("Title", "按班级");
            }
            dicts.Add("StudentAnalEvaluaChart", studentAnalEvaluaChart);
            dicts.Add("SAnalEvaluaChart", analEvaluaChart);
            //数据部分
            dicts.Add("Term", course.Semester);
            dicts.Add("CourseName", course.Name);
            dicts.Add("CourseType", course.Type);
            var claMajor = clas.Select(c => c.Major).Distinct().ToList();
            string claName = clas[0].SchoolYear;
            foreach (var item1 in claMajor)
            {
                claName = claName + item1;
                foreach (var item in clas.Where(c => c.Major == item1).OrderBy(c => c.Name))
                {
                    claName += Regex.Replace(item.Name, @"[^0-9]+", "");
                    claName += ",";
                }
                claName = claName.Substring(0, claName.Length - 1) + "班;";
            }
            claName = claName.Substring(0,claName.Length - 1);
            dicts.Add("StudentCLass", claName);
            dicts.Add("TeacherName", input.TeacherName);
            dicts.Add("TeacherName1", input.TeacherName);
            var examTime = LocalTool.TimeFormatStr(input.ExamTime, 4);
            dicts.Add("ExamTime", examTime);
            var allCompute = await ComputeDegreeAchieve(input.CourseId, input.ClassId);
            dicts.Add("Power0", exam.Power.ToString()+"%");
            for(var i=1;i<=4;i++)
            {
                var scoreWeightsName = "";
                var scoreWeightsPower = "";
                if (i <= scoreWeights.Count())
                {
                    scoreWeightsName = scoreWeights[i - 1].Name;
                    scoreWeightsPower = scoreWeights[i - 1].Power.ToString()+"%";
                }
                dicts.Add("ScoreWeight"+i, scoreWeightsName);
                dicts.Add("Power" + i, scoreWeightsPower);
            }
            dicts.Add("CourseDuration",course.ClassDuration.ToString());
            dicts.Add("TextDuration", course.TextDuration.ToString());
            dicts.Add("ExamPeople", students.Count().ToString());
            dicts.Add("TrueExamPeople", input.TrueExamPeople);
            //dicts.Add("BigQuestionCount",testQuestions.Count().ToString());
            //dicts.Add("LittleQuestionCount", questions.Count().ToString());
            dicts.Add("QuestionCount", input.QuestionCount.ToString());
            var allTable = await GetAllTable(input.CourseId, input.ClassId);
            int scorea1 = 0, scorea2=0, scorea3=0, scorea4=0, scorea5=0, scorea6=0;
            int scoreb1 = 0, scoreb2 = 0, scoreb3 = 0, scoreb4 = 0, scoreb5 = 0, scoreb6 = 0;
            var stuCount = allTable.DicsList.Count();
            List<double> Es = new List<double>();
            List<double> As = new List<double>();
            double avg1 = 0, standDe1 = 0, avg2 = 0, standDe2 = 0;
            foreach (var item in allTable.DicsList)
            {
                double realExamScore = (double)((float.Parse(item.GetValueOrDefault("allScore")) - float.Parse(item.GetValueOrDefault("usualScore")) * (1 - exam.Power / 100)) / exam.Power * 100);
                double realAllScore = float.Parse(item.GetValueOrDefault("allScore"));
                Es.Add(realExamScore);
                As.Add(realAllScore);
                //卷面成绩
                int examScore = (int)realExamScore;
                //总成绩
                int allScore = (int)realAllScore;
                avg1 = avg1 + realExamScore;
                avg2 = avg2+ realAllScore;
                switch (examScore/10)
                {
                    case 10:
                        scorea1++;break;
                    case 9:
                        scorea1++; break;
                    case 8:
                        scorea2++; break;
                    case 7:
                        scorea3++; break;
                    case 6:
                        scorea4++; break;
                    case 5:
                        scorea5++; break;
                    default:
                        scorea6++; break;
                }
                switch (allScore / 10)
                {
                    case 10:
                        scoreb1++; break;
                    case 9:
                        scoreb1++; break;
                    case 8:
                        scoreb2++; break;
                    case 7:
                        scoreb3++; break;
                    case 6:
                        scoreb4++; break;
                    case 5:
                        scoreb5++; break;
                    default:
                        scoreb6++; break;
                }
            }
            #region 计算各阶段分数，平均分和标准差
            dicts.Add("Numa1", scorea1.ToString());
            dicts.Add("Ratea1", Math.Round(((decimal)scorea1 / stuCount)*100, 1) + "%");
            dicts.Add("Numa2", scorea2.ToString());
            dicts.Add("Ratea2", Math.Round(((decimal)scorea2 / stuCount) * 100, 1) + "%");
            dicts.Add("Numa3", scorea3.ToString());
            dicts.Add("Ratea3", Math.Round(((decimal)scorea3 / stuCount) * 100, 1) + "%");
            dicts.Add("Numa4", scorea4.ToString());
            dicts.Add("Ratea4", Math.Round(((decimal)scorea4 / stuCount) * 100, 1) + "%");
            dicts.Add("Numa5", scorea5.ToString());
            dicts.Add("Ratea5", Math.Round(((decimal)scorea5 / stuCount) * 100, 1) + "%");
            dicts.Add("Numa6", scorea6.ToString());
            dicts.Add("Ratea6", Math.Round(((decimal)scorea6 / stuCount) * 100, 1) + "%");
            dicts.Add("Numb1", scoreb1.ToString());
            dicts.Add("Rateb1", Math.Round(((decimal)scoreb1 / stuCount) * 100, 1) + "%");
            dicts.Add("Numb2", scoreb2.ToString());
            dicts.Add("Rateb2", Math.Round(((decimal)scoreb2 / stuCount) * 100, 1) + "%");
            dicts.Add("Numb3", scoreb3.ToString());
            dicts.Add("Rateb3", Math.Round(((decimal)scoreb3 / stuCount) * 100, 1) + "%");
            dicts.Add("Numb4", scoreb4.ToString());
            dicts.Add("Rateb4", Math.Round(((decimal)scoreb4 / stuCount) * 100, 1) + "%");
            dicts.Add("Numb5", scoreb5.ToString());
            dicts.Add("Rateb5", Math.Round(((decimal)scoreb5 / stuCount) * 100, 1) + "%");
            dicts.Add("Numb6", scoreb6.ToString());
            dicts.Add("Rateb6", Math.Round(((decimal)scoreb6 / stuCount) * 100, 1) + "%");
            dicts.Add("Avg1", Math.Round((avg1 / stuCount),1).ToString());
            dicts.Add("Avg2", Math.Round((avg2 / stuCount),1).ToString());
            var realAvg1 = avg1 / stuCount;
            var realAvg2 = avg2 / stuCount;
            foreach (var item in Es)
            {
                standDe1 += Math.Pow(item - realAvg1,2);
            }
            foreach (var item in As)
            {
                standDe2 += Math.Pow(item - realAvg2,2);
            }
            dicts.Add("StandDe1", Math.Round(Math.Sqrt(standDe1),1).ToString());
            dicts.Add("StandDe2", Math.Round(Math.Sqrt(standDe2),1).ToString());
            #endregion
            dicts.Add("AnalEvalua", input.AnalEvalua);
            dicts.Add("StudentAnalEvalua", input.StudentAnalEvalua);
            dicts.Add("Problem1", input.Problem1);
            dicts.Add("Assess", input.Assess);
            dicts.Add("Problem2", input.Problem2);
            dicts.Add("Improve", input.Improve);
            string tempFilePath = Path.Combine(_env.WebRootPath, "FileTemplate/班级达成评价表.docx");
            string outFilePath = Path.Combine(_env.WebRootPath, "ExportExcels", LocalTool.GetTimeStamp(DateTime.Now) + ".docx");
            WordHelp.ExportChartWord(tempFilePath, outFilePath, dicts, courObjCreateTable, courObjCreateTableForMajor);
            //WordHelp.AddTable(outFilePath);
            return new List<string>
            {
                excleTable,
                _fileManagementAppService.PathToRelative(outFilePath)
        };
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
        /// <summary>
        /// 试卷审核表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> ExaminationReview(ExaminationReviewDto input)
        {
            //导出出现问题方法内矫正
            if(input.WingdingsIsA == 1)
            {
                input.WingdingsIsA = 2;
            }
            else
            {
                input.WingdingsIsA = 1;
            }
            if (input.WingdingsIsAS == 1)
            {
                input.WingdingsIsAS = 2;
            }
            else
            {
                input.WingdingsIsAS = 1;
            }
            Dictionary<string, string> data = input.ToStringDictionary();
            Dictionary<string,string> dicts = new Dictionary<string, string>();
            Dictionary<string,string> list = new Dictionary<string, string>();
            var course = await _courseEFRepository.FirstOrDefaultAsync(c => c.Id == input.CourseId);
            var claCourseIds = await _classCourseEFRepository.GetAll().Where(c => c.CourseId == input.CourseId).Select(c => c.ClassId).ToListAsync();
            var students = await _studentEFRepository.GetAll().Where(c => claCourseIds.Contains(c.ClassId)).ToListAsync();
            var clas = await _classesEFRepository.GetAll().Where(c => claCourseIds.Contains(c.Id)).ToListAsync();//所有班级
            var claMajor = clas.Select(c=>c.Major).Distinct().ToList();
            string claName = clas[0].SchoolYear;
            foreach (var item1 in claMajor)
            {
                claName = claName +  item1;
                foreach (var item in clas.Where(c=>c.Major == item1).OrderBy(c => c.Name))
                {
                    claName += Regex.Replace(item.Name, @"[^0-9]+", "");
                    claName += ",";
                }
                claName = claName.Substring(0, claName.Length - 1) + "班;";
            }
            dicts.Add("CourseName", course.Name);
            dicts.Add("MajorClass", claName.Substring(0, claName.Length - 1));
            dicts.Add("Semester", course.Semester);
            var examTime = LocalTool.TimeFormatStr(input.ExamTime, 4);
            dicts.Add("ExamTime", examTime);
            dicts.Add("StudentCount", students.Count().ToString());
            dicts.Add("ExamTable1", "");
            dicts.Add("ExamTable2", "");
            foreach (var item in data)
            {
                var s = item.Key.ToString();
                if (item.Key.ToString().Contains("Wingdings"))
                {
                    list.Add(item.Key,item.Value);
                }
            }
            foreach(var item in list)
            {
                for(var i=1; i<=3; i++)
                {
                    if(item.Value == i.ToString())
                    {
                        dicts.Add(item.Key+i, "82");
                    }
                    else
                    {
                        dicts.Add(item.Key + i, "163");
                    }
                }
            }
            string tempFilePath = Path.Combine(_env.WebRootPath, "FileTemplate/试卷审核表.docx");
            string outFilePath = Path.Combine(_env.WebRootPath, "ExportExcels", LocalTool.GetTimeStamp(DateTime.Now) + ".docx");
            WordHelp.ExportExam(tempFilePath, outFilePath, dicts, input.ExamTable1, input.ExamTable2);
            return _fileManagementAppService.PathToRelative(outFilePath);
        }
    }
}
