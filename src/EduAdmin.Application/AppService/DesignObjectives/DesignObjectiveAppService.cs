using Abp.Authorization;
using Abp.Domain.Repositories;
using AutoMapper;
using EduAdmin.AppService.DesignObjectives.Dto;
using EduAdmin.AppService.ScoreAchievements.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.DesignObjectives
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class DesignObjectiveAppService:EduAdminAppServiceBase,IDesignObjectiveAppService
    {
        private readonly IRepository<GraduationRequirement, Guid> _graduationRequirementEFRepository;
        private readonly IRepository<DesignObjective, Guid> _designObjectiveEFRepository;
        private readonly IRepository<ScoreWeight, Guid> _scoreWeightEFRepository;
        public DesignObjectiveAppService(
            IRepository<GraduationRequirement, Guid> graduationRequirementEFRepository,
            IRepository<DesignObjective, Guid> designObjectiveEFRepository,
            IRepository<ScoreWeight, Guid> scoreWeightEFRepository)
        {
            _graduationRequirementEFRepository = graduationRequirementEFRepository;
            _designObjectiveEFRepository = designObjectiveEFRepository;
            _scoreWeightEFRepository = scoreWeightEFRepository;
        }
        /// <summary>
        /// 获取所有课设目标
        /// </summary>
        /// <returns></returns>
        public async Task<List<DesignObjectiveShowDto>> GetAllDesignObjective(Guid outlineId)
        {
            List<DesignObjectiveShowDto> list = new List<DesignObjectiveShowDto>();
            var graReq = await _graduationRequirementEFRepository.GetAllListAsync();
            var designObjectiveList = await _designObjectiveEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);

            foreach (var designObjective in designObjectiveList)
            {
                var gra = graReq.FirstOrDefault(c => c.Id == designObjective.GraduationRequirementId);
                List<ScoreAchievementShowDto> scoreRate = new List<ScoreAchievementShowDto>();
                if (designObjective.ScoreProportion != null)
                    scoreRate = JsonConvert.DeserializeObject<List<ScoreAchievementShowDto>>(designObjective.ScoreProportion);
                list.Add(new DesignObjectiveShowDto
                {
                    Id = designObjective.Id,
                    DegreeSupport = designObjective.DegreeSupport,
                    GraduationRequirement = gra?.Name,
                    GredeProportion = designObjective.GredeProportion,
                    GraduationRequirementId = designObjective.GraduationRequirementId,
                    ScoreRate = scoreRate,
                    Name = designObjective.Name,
                });
            }
            return list;
        }

        /// <summary>
        /// 添加课设目标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddDesignObjective(CreateDesignObjectiveDto input)
        {
            var id = await _designObjectiveEFRepository.InsertAndGetIdAsync(new DesignObjective
            {
                OutlineId = input.OutlineId
            });
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改课设目标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateDesignObjective(CreateDesignObj input)
        {
            var get = await _designObjectiveEFRepository.GetAsync(input.Id);
            get.Id = input.Id;
            get.ScoreProportion = JsonConvert.SerializeObject(input.ScoreRate);
            get.DegreeSupport = input.DegreeSupport;
            get.GraduationRequirementId = input.GraduationRequirementId;
            get.GredeProportion = input.GredeProportion;
            get.Name = input.Name;
            return new UpdateResult();
        }
        /// <summary>
        /// 删除课设目标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteDesignObjective(Guid id)
        {
            await _designObjectiveEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
        /// <summary>
        /// 获取课设目标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DesignObjectiveShowDto> GetCourse(Guid id)
        {
            var designObjective = await _designObjectiveEFRepository.GetAsync(id);
            return ObjectMapper.Map<DesignObjectiveShowDto>(designObjective);
        }
        
    }
}
