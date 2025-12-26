using Abp.Authorization;
using Abp.Domain.Repositories;
using AutoMapper;
using EduAdmin.AppService.CourseObjectives.Dto;
using EduAdmin.AppService.ScoreWeights.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreWeights
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class ScoreWeightAppService:EduAdminAppServiceBase,IScoreWeightAppService
    {
        private readonly IRepository<Teacher, Guid> _teacherEFRepository;
        private readonly IRepository<ScoreWeight, Guid> _scoreWeightEFRepository;
        private readonly IRepository<Homework, Guid> _homeWorkEFRepository;
        private readonly IRepository<SwDetail, Guid> _swDetailEFRepository;
        public ScoreWeightAppService(
            IRepository<Teacher, Guid> teacherEFRepository,
            IRepository<ScoreWeight, Guid> ScoreWeightEFRepository,
            IRepository<Homework, Guid> homeWorkEFRepository,
            IRepository<SwDetail, Guid> swDetailEFRepository)
        {
            _teacherEFRepository = teacherEFRepository;
            _scoreWeightEFRepository = ScoreWeightEFRepository;
            _homeWorkEFRepository = homeWorkEFRepository;
            _swDetailEFRepository = swDetailEFRepository;
        }
        /// <summary>
        /// 获取所有权重
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreWeightShowDto>> GetAllScoreWeight(Guid outlineId)
        {
            var ScoreWeightList = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            return ObjectMapper.Map<List<ScoreWeightShowDto>>(ScoreWeightList);
        }
        /// <summary>
        /// 获取所有权重
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreWeightShowDto>> GetUsualScoreWeight(Guid outlineId)
        {
            var ScoreWeightList = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId && c.Name != "期末考试");
            return ObjectMapper.Map<List<ScoreWeightShowDto>>(ScoreWeightList);
        }
        /// <summary>
        /// 添加权重
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddScoreWeight(CreateScoreWeightDto input)
        {
            var outlineSw = await _scoreWeightEFRepository.GetAllListAsync(c=>c.OutlineId == input.OutlineId);
            if (outlineSw.Count() >= 6)
            {
                return new AddResult<Guid>("上限6个");
            }
            var ScoreWeight = ObjectMapper.Map<ScoreWeight>(input);
            var id = await _scoreWeightEFRepository.InsertAndGetIdAsync(ScoreWeight);
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改权重
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateScoreWeight(UpdateScoreWeight input)
        {
            var ScoreWeight = ObjectMapper.Map<ScoreWeight>(input);
            //添加作业
            if(!string.IsNullOrEmpty(input.Name) && input.Times != null && input.Power !=null)
            {
                await _swDetailEFRepository.DeleteAsync(c => c.ScoreWeightId == input.Id);
                float? a = 0;
                if(input.Power != 0 && input.Times != 0)
                {
                    a = (float?)Math.Round((decimal)(input.Power / input.Times), 1);
                }
                for (var i = 1; i <= input.Times; i++)
                {
                    await _swDetailEFRepository.InsertAsync(new SwDetail
                    {
                        OutlineId = input.OutlineId,
                        ScoreWeightId = input.Id,
                        Name = input.Name+i,
                        Power = a,
                        Times = i
                    });
                }
            }
            await _scoreWeightEFRepository.UpdateAsync(ScoreWeight);
            return new UpdateResult();
        }
        /// <summary>
        /// 删除权重
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteScoreWeight(Guid id)
        {
            //删除作业
            await _swDetailEFRepository.DeleteAsync(c=>c.ScoreWeightId == id
            );
            //删除权重
            await _scoreWeightEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
        /// <summary>
        /// 返回成绩权重的名称
        /// </summary>
        /// <param name="outlineId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetScoreWeightName(Guid outlineId)
        {
            var scoreWeight = await _scoreWeightEFRepository.GetAll().Where(c=>c.OutlineId == outlineId).Select(c=>c.Name).ToListAsync();
            return scoreWeight;
        }
        /// <summary>
        /// 返回作业权重次数列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="scoreWeightId"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<int>>> GetScoreWeightTimes(Guid courseId,Guid scoreWeightId)
        {
            List<SelectDto<int>> list = new List<SelectDto<int>>();
            var scoreWeight = await _scoreWeightEFRepository.GetAsync(scoreWeightId);
            var times = await _homeWorkEFRepository.GetAll().Where(c => c.CourseId == courseId && c.Type == scoreWeightId.ToString()).Select(c => c.Times).ToListAsync();
            for(var i = 1; i <= scoreWeight.Times; i++)
            {
                if (!times.Contains(i))
                {
                    list.Add(new SelectDto<int>(i, i.ToString()));
                }
            }
           return list;
        }
        /// <summary>
        /// 获取权重详细
        /// </summary>
        /// <param name="outlineId"></param>
        /// <returns></returns>
        public async Task<List<SwDetailShowDto>> GetAllSwDetail(Guid outlineId)
        {
            List<SwDetailShowDto> list = new List<SwDetailShowDto>();
            var scoreWeights = await _scoreWeightEFRepository.GetAllListAsync(c => c.OutlineId == outlineId && c.Name != "期末考试");
            var swDetailList = await _swDetailEFRepository.GetAllListAsync(c=>c.OutlineId == outlineId);
            foreach(var scoreWeight in scoreWeights)
            {
                List<SwDetailList> list1 = new List<SwDetailList>();
                var swDetails = swDetailList.Where(c => c.ScoreWeightId == scoreWeight.Id);
                foreach(var item in swDetails)
                {
                    list1.Add(new SwDetailList
                    {
                        SwDetailId = item.Id,
                        SwDetailName = item.Name,
                        SwDetailPower = item.Power,
                        CourseObjectiveId = item.CourseObjectiveId
                    });
                }
                list.Add(new SwDetailShowDto
                {
                    ScoreWeightName = scoreWeight.Name,
                    SwDetails = list1
                });
            }
            return list;
        }
        /// <summary>
        /// 更改权重详细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateSwDetail(UpdateSwDetailDto input)
        {
            var swDetail = await _swDetailEFRepository.GetAsync(input.Id);
            swDetail.CourseObjectiveId = input.CourseObjectiveId;
            return new UpdateResult();
        }
    }
}
