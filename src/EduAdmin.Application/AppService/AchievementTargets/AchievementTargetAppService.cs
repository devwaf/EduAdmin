using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using EduAdmin.AppService.AchievementTargets.Dto;
using EduAdmin.AppService.ScoreAchievements.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.AchievementTargets
{
    /// <summary>
    /// 评审指标
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AchievementTargetAppService : EduAdminAppServiceBase, IAchievementTargetAppService
    {
        private readonly IRepository<AchievementTarget, Guid> _achievementTargetEFRepository;
        private readonly IRepository<ScoreAchievement, Guid> _scoreAchievementEFRepository;
        private readonly IRepository<Outline, Guid> _outlineEFRepository;
        private readonly IRepository<DesignObjective, Guid> _designObjectiveEFRepository;
        public AchievementTargetAppService(
            IRepository<AchievementTarget, Guid> achievementTargetEFRepository,
            IRepository<ScoreAchievement, Guid> scoreAchievementEFRepository,
            IRepository<DesignObjective, Guid> designObjectiveEFRepository,
            IRepository<Outline, Guid> outlineEFRepository)
        {
            _achievementTargetEFRepository = achievementTargetEFRepository;
            _scoreAchievementEFRepository = scoreAchievementEFRepository;
            _designObjectiveEFRepository = designObjectiveEFRepository;
            _outlineEFRepository = outlineEFRepository;
        }
        /// <summary>
        /// 获取所有评审指标
        /// </summary>
        /// <returns></returns>
        public async Task<List<AchievementTargetShowDto>> GetAllAchievementTarget(Guid scoreAchievementId)
        {
            var achievementTargetList = await _achievementTargetEFRepository.GetAllListAsync(c =>c.ScoreAchievementId == scoreAchievementId);
            return ObjectMapper.Map<List<AchievementTargetShowDto>>(achievementTargetList);
        }
        /// <summary>
        /// 获取剩余评审指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<AchievementTargetShowDto>> GetHasAchievementTarget(ShowHasAchiveTargetInput input)
        {
            List<AchievementTargetShowDto> list = new List<AchievementTargetShowDto>();
            var scoreAches = await _scoreAchievementEFRepository.GetAllListAsync(c => c.OutlineId == input.OutlineId);
            var achiveTargets = await _achievementTargetEFRepository.GetAllListAsync();
            var deObj = await _designObjectiveEFRepository.GetAllListAsync(c=>c.OutlineId == input.OutlineId);
            List<Guid> list1 = new List<Guid>();
            foreach(var item in deObj)
            {
                if (item.ScoreProportion == null) continue;
              var scoreRates = JsonConvert.DeserializeObject<List<ScoreAchievementShowDto>>(item.ScoreProportion);
                foreach(var ite in scoreRates)
                {
                   foreach(var it in ite.AchieveTarget)
                    {
                        list1.Add(it.Id);
                    }
                }
            }
            foreach (var scoreAchievement in scoreAches)
            {
                var achiveTarget = achiveTargets.Where(c => c.ScoreAchievementId == scoreAchievement.Id);
                var has = achiveTarget.ToList();
                var hasList = ObjectMapper.Map<List<AchievementTargetShowDto>>(has);
                foreach (var it in hasList)
                {
                    if(list1.Contains(it.Id))
                        it.IsActive = true;
                    else
                        it.IsActive = false;
                }
                list.AddRange(hasList);
            }
            return list;
        }
        /// <summary>
        /// 添加评审指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddAchievementTarget(CreateAchievementTargetDto input)
        {
           var achievement = ObjectMapper.Map<AchievementTarget>(input);
           var id = await _achievementTargetEFRepository.InsertAndGetIdAsync(achievement);
           var scoreAchievement = await _scoreAchievementEFRepository.GetAsync(input.ScoreAchievementId);
            scoreAchievement.Weight = input.rate;
           return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改评审指标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateAchievementTarget(CreateAchievementTargetDto input)
        {
            var achievement = ObjectMapper.Map<AchievementTarget>(input);
            await _achievementTargetEFRepository.UpdateAsync(achievement);
            return new UpdateResult();
        }
        /// <summary>
        /// 删除评审指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteAchievementTarget(Guid id)
        {
            await _achievementTargetEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
        /// <summary>
        /// 获取评审指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AchievementTargetShowDto> GetAchievementTarget(Guid id)
        {
            var achievement = await _achievementTargetEFRepository.GetAsync(id);
            return ObjectMapper.Map<AchievementTargetShowDto>(achievement);
        }
        /// <summary>
        /// 验证课设完整性
        /// </summary>
        /// <param name="outlineId"></param>
        /// <returns></returns>
        public async Task<ResultDto> CheckDesignOutLine(Guid outlineId)
        {
            var outline = await _outlineEFRepository.GetAsync(outlineId);
            var allWeight =  _scoreAchievementEFRepository.GetAll().Where(c => c.OutlineId == outlineId).ToList();
            var target = await _achievementTargetEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            var targetNum = target.Select(c => c.Score).Sum();
            var weightNum = allWeight.Select(c => c.Weight).Sum();
            var designObj = await _designObjectiveEFRepository.GetAllListAsync(C=>C.OutlineId == outlineId);
            if(designObj.Count() == 0)
            {
                outline.IsComplete = false;
                return new ResultDto(false,"请将课设目标填写完整");
            }
            if (weightNum != 100f)
            {
                outline.IsComplete = false;
                return new ResultDto(false,"评审项目占比之和不为100");
            }
            if(targetNum != 100)
            {
                outline.IsComplete = false;
                return new ResultDto(false, "评审项目指标的分数之和不为100");
            }
            var res = await  GetHasAchievementTarget(new ShowHasAchiveTargetInput { OutlineId  = outlineId});
            if(res.FirstOrDefault(c=>c.IsActive == false) != null)
            {
                outline.IsComplete = false;
                return new ResultDto(false, "对照表指标与分项成绩表不符");
            }
            //大纲完整
            outline.IsComplete = true;
            return new ResultDto(true, "大纲通过完整性验证");
        }
    }
}
