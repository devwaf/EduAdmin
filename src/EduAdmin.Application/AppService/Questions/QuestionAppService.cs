using Abp.Authorization;
using Abp.Domain.Repositories;
using EduAdmin.AppService.CourseObjectives.Dto;
using EduAdmin.AppService.Questions.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Questions
{
    /// <summary>
    /// 小题
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class QuestionAppService : EduAdminAppServiceBase, IQuestionAppService
    {
        private readonly IRepository<Teacher, Guid> _teacherEFRepository;
        private readonly IRepository<Question, Guid> _questionEFRepository;
        private readonly IRepository<TestQuestion, Guid> _testQuestionEFRepository;
        private readonly IRepository<CourseObjective, Guid> _courseObjectiveEFRepository;
        private readonly IRepository<Course, Guid> _courseEFRepository;
        public QuestionAppService(
            IRepository<Teacher, Guid> teacherEFRepository,
            IRepository<Question, Guid> questionEFRepository,
            IRepository<CourseObjective, Guid> courseObjectiveEFRepository,
            IRepository<TestQuestion, Guid> testQuestionEFRepository,
        IRepository<Course, Guid> courseEFRepository)
        {
            _teacherEFRepository = teacherEFRepository;
            _questionEFRepository = questionEFRepository;
            _courseObjectiveEFRepository = courseObjectiveEFRepository;
            _courseEFRepository = courseEFRepository;
            _testQuestionEFRepository = testQuestionEFRepository;
        }
        /// <summary>
        /// 获取所有小题
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuestionShowDto>> GetAllQuestion(Guid outlineId)
        {
            List<QuestionShowDto> list = new List<QuestionShowDto>();
            var coureObj = await _courseObjectiveEFRepository.GetAllListAsync();
            var questionList = await _questionEFRepository.GetAllListAsync(c =>c.OutlineId == outlineId);
            foreach (var question in questionList)
            {
                var courseObj = coureObj.FirstOrDefault(c => c.Id == question.CourseObjectiveId);
                list.Add(new QuestionShowDto
                {
                    Id = question.Id,
                    TestQuestionId = question.TestQuestionId,
                    Score = question.Score,
                    TitleNum = question.TitleNum,
                    CourseObjectiveName = courseObj?.Name,
                    CourseObjectiveId = courseObj?.Id
                }) ;
            }
            return list;
        }
        ///// <summary>
        ///// 添加小题
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<AddResult<Guid>> AddQuestion(CreateQuestionDto input)
        //{
        //    var question = ObjectMapper.Map<Question>(input);
        //    var id = await _questionEFRepository.InsertAndGetIdAsync(question);
        //    return new AddResult<Guid>(id);
        //}
        /// <summary>
        /// 添加小题
        /// </summary>
        /// <param name="outline"></param>
        /// <param name="testQuestionId"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddQuestion(Guid outline, Guid testQuestionId)
        {
            var testque = await _testQuestionEFRepository.FirstOrDefaultAsync(c => c.Id == testQuestionId);
            var question = await _questionEFRepository.FirstOrDefaultAsync(c => c.TitleNum == testque.TitleNum && c.OutlineId == outline);
            if(question != null)
            {
                return new AddResult<Guid>("大题有课程目标不能添加小题");
            }
            var id = await _questionEFRepository.InsertAndGetIdAsync(new Question
            {
                OutlineId = outline,
                TestQuestionId = testQuestionId
            });
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改小题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateQuestion(CreateQuestionDto input)
        {
            var questionGet = await _questionEFRepository.FirstOrDefaultAsync(c=>c.Id == input.Id);
            var testQue = await _testQuestionEFRepository.FirstOrDefaultAsync(c => c.Id == questionGet.TestQuestionId);
            if(input.TitleNum == testQue.TitleNum)
            {
                return new UpdateResult("小题名称不能和大题相同");
            }
            //var question = ObjectMapper.Map<Question>(input);
            questionGet.TestQuestionId = input.TestQuestionId;
            questionGet.CourseObjectiveId = input.CourseObjectiveId;
            questionGet.Score = input.Score;
            questionGet.TitleNum = input.TitleNum;
            questionGet.OutlineId = questionGet.OutlineId;
            await _questionEFRepository.UpdateAsync(questionGet);
            return new UpdateResult();
        }
        /// <summary>
        /// 删除小题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteQuestion(Guid id)
        {
            await _questionEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
        /// <summary>
        /// 获取小题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QuestionShowDto> GetQuestion(Guid id)
        {
            var question = await _questionEFRepository.GetAsync(id);
            return ObjectMapper.Map<QuestionShowDto>(question);
        }
        /// <summary>
        /// 试卷审核表中的课程目标与考试分值
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<List<CoureObjAndQuestionScoreDto>> GetCoureObjAndQuestionScore(Guid courseId)
        {
            List<CoureObjAndQuestionScoreDto> list = new List<CoureObjAndQuestionScoreDto>();
            var outlineId = (await _courseEFRepository.FirstOrDefaultAsync(c => c.Id == courseId)).OutlineId;
            var questions = await _questionEFRepository.GetAllListAsync(c=>c.OutlineId == outlineId);//当前课程的所有小题
            var courseObjs = await _courseObjectiveEFRepository.GetAllListAsync(c=>c.OutlineId==outlineId);
            foreach(var courseObj in courseObjs)
            {
                List<string> list1 = new List<string>();
                var ques = questions.Where(c => c.CourseObjectiveId == courseObj.Id);
                foreach(var que in ques)
                {
                    list1.Add(que.TitleNum + "(" + que.Score + ")");
                }
                list.Add(new CoureObjAndQuestionScoreDto
                {
                    ObjContent =" "+  courseObj.Name+ ":"+ courseObj.Content,
                    QuestionScore = list1
                });
            }
            return list;
        }
    }
}
