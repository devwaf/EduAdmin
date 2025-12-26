using Abp.Authorization;
using Abp.Domain.Repositories;
using EduAdmin.AppService.Questions;
using EduAdmin.AppService.TestQuestions.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.TestQuestions
{
    /// <summary>
    /// 教师
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class TestQuestionAppService : EduAdminAppServiceBase, ITestQuestionAppService
    {
        private readonly IQuestionAppService _questionAppService;
        private readonly IRepository<TestQuestion, Guid> _testQuestionEFRepository;
        private readonly IRepository<Question, Guid> _questionEFRepository;
        private readonly IRepository<CourseObjective, Guid> _courseObjectiveEFRepository;
        public TestQuestionAppService(
            IQuestionAppService questionAppService,
            IRepository<TestQuestion, Guid> courseEFRepository,
            IRepository<Question, Guid> questionEFRepository,
            IRepository<CourseObjective, Guid> courseObjectiveEFRepository)
        {
            _questionAppService = questionAppService;
            _testQuestionEFRepository = courseEFRepository;
            _questionEFRepository = questionEFRepository;
            _courseObjectiveEFRepository = courseObjectiveEFRepository;
        }
        /// <summary>
        /// 获取所有大题
        /// </summary>
        /// <returns></returns>
        public async Task<TestQueAccount> GetAllTestQuestion(Guid outlineId)
        {
            List<TestQuestionShowDto> list = new List<TestQuestionShowDto>();
            var courseObjs = await _courseObjectiveEFRepository.GetAllListAsync(c=>c.OutlineId == outlineId);
            //大纲中所有的小题
            var allQue = await _questionAppService.GetAllQuestion(outlineId);
            //所有小题的个数
            var count = allQue.Count();
            //所有小题的分数之和
            var scoreNum = allQue.Select(c=>c.Score).Sum();
            var testQuestionList = await _testQuestionEFRepository.GetAllListAsync(c =>c.OutlineId == outlineId);
            foreach(var testQuestion in testQuestionList)
            {
                //当前大题的所有小题
                var aQue = allQue.Where(c => c.TestQuestionId == testQuestion.Id && c.TitleNum != testQuestion.TitleNum).ToList();
                var firstQue = allQue.Where(c => c.TestQuestionId == testQuestion.Id && c.TitleNum == testQuestion.TitleNum).FirstOrDefault();
                Guid? couObjId = null;
                string couObjName = null;
                if(firstQue != null)
                {
                    couObjId = firstQue.CourseObjectiveId;
                    couObjName = courseObjs.FirstOrDefault(c => c.Id == couObjId)?.Name;
                }   
                list.Add(new TestQuestionShowDto
                {
                  Id = testQuestion.Id,
                  Question = aQue,
                  Score = testQuestion.Score,
                  TitleNum = testQuestion.TitleNum,
                  Type =  testQuestion.Type,
                  CourseObjectiveId = couObjId,
                  CourseObjName = couObjName,
                });
            }
            return new TestQueAccount
            {
                Count = count,
                ScoreNum = (int)scoreNum,
                TestQuestions = list
            };
        }
        ///// <summary>
        ///// 添加大题
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<AddResult<Guid>> AddTestQuestion(CreateTestQuestionDto input)
        //{
        //   var testQuestion = ObjectMapper.Map<TestQuestion>(input);
        //   var id = await _testQuestionEFRepository.InsertAndGetIdAsync(testQuestion);
        //    foreach(var item in input.Question)
        //    {
        //        var question = ObjectMapper.Map<Question>(item);
        //        question.OutlineId = input.OutlineId;
        //        question.TestQuestionId = id;
        //        await _questionEFRepository.InsertAsync(question);
        //    }
        //   return new AddResult<Guid>(id);
        //}
        ///// <summary>
        ///// 修改大题
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<UpdateResult> UpdateTestQuestion(CreateTestQuestionDto input)
        //{
        //    var testQuestion = ObjectMapper.Map<TestQuestion>(input);
        //    await _testQuestionEFRepository.UpdateAsync(testQuestion);
        //    return new UpdateResult();
        //}
        public async Task<AddResult<Guid>> AddTestQuestion(Guid OutlineId,string testQuestionName)
        {
            var id = await _testQuestionEFRepository.InsertAndGetIdAsync(new TestQuestion
            {
                OutlineId = OutlineId,
                TitleNum = testQuestionName
            });
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改大题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateTestQuestion(CreateTestQuestionDto input)
        {
            var testQuestion = ObjectMapper.Map<TestQuestion>(input);
            await _testQuestionEFRepository.UpdateAsync(testQuestion);
            if(input.CourseObjectiveId == null)
            {
                await _questionEFRepository.DeleteAsync(c => c.TitleNum == testQuestion.TitleNum && c.TestQuestionId == testQuestion.Id);
            }
            if(input.CourseObjectiveId != null && input.TitleNum != null && input.Type != null && input.Score != null)
            {
                await _questionEFRepository.DeleteAsync(c => c.TitleNum == input.TitleNum && c.TestQuestionId == input.Id);
                await _questionEFRepository.InsertAsync(new Question
                {
                    TitleNum = input.TitleNum,
                    Score = input.Score,
                    CourseObjectiveId = input.CourseObjectiveId,
                    TestQuestionId = input.Id,
                    OutlineId = input.OutlineId
                });
            }
            return new UpdateResult();
        }
        /// <summary>
        /// 删除大题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteTestQuestion(Guid id)
        {
            //删除所有小题
            await _questionEFRepository.DeleteAsync(c=>c.TestQuestionId == id);
            await _testQuestionEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
        /// <summary>
        /// 获取大题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TestQuestionShowDto> GetTestQuestion(Guid id)
        {
            var testQuestion = await _testQuestionEFRepository.GetAsync(id);
            return ObjectMapper.Map<TestQuestionShowDto>(testQuestion);
        }
    }
}
