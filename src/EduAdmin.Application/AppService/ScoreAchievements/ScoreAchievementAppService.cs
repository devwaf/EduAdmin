using Abp.Authorization;
using Abp.Domain.Repositories;
using EduAdmin.AppService.AchievementTargets;
using EduAdmin.AppService.AchievementTargets.Dto;
using EduAdmin.AppService.ScoreAchievements.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreAchievements
{
    /// <summary>
    /// 成绩评审
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class ScoreAchievementAppService : EduAdminAppServiceBase, IScoreAchievementAppService
    {
        private readonly IRepository<ScoreAchievement, Guid> _scoreAchievementEFRepository;
        private readonly IRepository<AchievementTarget, Guid> _achievementTargetEFRepository;
        public ScoreAchievementAppService(
            IRepository<ScoreAchievement, Guid> courseEFRepository,
            IRepository<AchievementTarget, Guid> achievementTargetEFRepository)
        {
            _scoreAchievementEFRepository = courseEFRepository;
            _achievementTargetEFRepository = achievementTargetEFRepository;
        }
        /// <summary>
        /// 获取所有成绩评审
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreAchievementShowDto>> GetAllScoreAchievement(Guid outlineId)
        {
            List<ScoreAchievementShowDto> list = new List<ScoreAchievementShowDto>();
            var scoreAchievementList = await _scoreAchievementEFRepository.GetAllListAsync(c =>c.OutlineId == outlineId);
            var achievementTargetList = await _achievementTargetEFRepository.GetAllListAsync();
            foreach (var scoreAchievement in scoreAchievementList)
            {
                var achieveTarget = achievementTargetList.Where(c => c.ScoreAchievementId == scoreAchievement.Id).ToList();
                //评审指标点
                var show = ObjectMapper.Map<List<AchievementTargetShowDto>>(achieveTarget);
                list.Add(new ScoreAchievementShowDto
                {
                    Id = scoreAchievement.Id,
                    Name = scoreAchievement.Name,
                    Weight = scoreAchievement.Weight,
                    AchieveTarget = show
                });
            }
            return list;
        }
        /// <summary>
        /// 添加成绩评审
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddScoreAchievement(CreateScoreAchievementDto input)
        {
           var scoreAchievement = ObjectMapper.Map<ScoreAchievement>(input);
           var id = await _scoreAchievementEFRepository.InsertAndGetIdAsync(scoreAchievement);
           return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改成绩评审
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateScoreAchievement(CreateScoreAchievementDto input)
        {
            var scoreAchievement = ObjectMapper.Map<ScoreAchievement>(input);
            await _scoreAchievementEFRepository.UpdateAsync(scoreAchievement);
            return new UpdateResult();
        }
        /// <summary>
        /// 删除成绩评审
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteScoreAchievement(Guid id)
        {
            await _scoreAchievementEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
    }
}
