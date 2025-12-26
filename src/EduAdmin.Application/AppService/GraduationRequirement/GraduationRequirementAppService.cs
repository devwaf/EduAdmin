using Abp.Domain.Repositories;
using AutoMapper;
using EduAdmin.AppService.GraduationRequirements.Dto;
using EduAdmin.AppService.Targets;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.GraduationRequirements
{
    public class GraduationRequirementAppService:EduAdminAppServiceBase,IGraduationRequirementAppService
    {
        private readonly IRepository<Target, Guid> _targetEFRepository;
        private readonly IRepository<GraduationRequirement, Guid> _graduationRequirementEFRepository;
        private readonly IRepository<CourseObjective, Guid> _courseObjectiveEFRepository;
        public GraduationRequirementAppService(
            IRepository<Target, Guid> targetEFRepository,
            IRepository<GraduationRequirement, Guid> graduationRequirementEFRepository,
            IRepository<CourseObjective, Guid> courseObjectiveEFRepository)
        {
            _targetEFRepository = targetEFRepository;
            _graduationRequirementEFRepository = graduationRequirementEFRepository;
            _courseObjectiveEFRepository = courseObjectiveEFRepository;
        }
        /// <summary>
        /// 获取所有毕业要求
        /// </summary>
        /// <returns></returns>
        public async Task<List<GraduationRequirementShowDto>> GetAllGraduationRequirement()
        {
            List<GraduationRequirementShowDto> list = new List<GraduationRequirementShowDto>();
            var target = await _targetEFRepository.GetAllListAsync();
            var graduationRequirementList = await _graduationRequirementEFRepository.GetAllListAsync();
            foreach(var graRequire in graduationRequirementList)
            {
                var tarList = await _targetEFRepository.GetAllListAsync(c => c.GraduationRequireId == graRequire.Id);
                var tar = ObjectMapper.Map<List<TargetShowDto>>(tarList);
                list.Add(new GraduationRequirementShowDto
                {
                    Id = graRequire.Id,
                    Name = graRequire.Name,
                    Require = graRequire.Require,
                    Target = tar
                });
            }
            return list;
        }
        /// <summary>
        /// 添加毕业要求
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddGraduationRequirement(CreateGraduationRequirementDto input)
        {

            var graduationRequirement = ObjectMapper.Map<GraduationRequirement>(input);
            var id = await _graduationRequirementEFRepository.InsertAndGetIdAsync(graduationRequirement);
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改毕业要求
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateGraduationRequirement(CreateGraduationRequirementDto input)
        {
            var graduationRequirement = ObjectMapper.Map<GraduationRequirement>(input);
            await _graduationRequirementEFRepository.UpdateAsync(graduationRequirement);
            return new UpdateResult();
        }
        /// <summary>
        /// 删除毕业要求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteGraduationRequirement(Guid id)
        {
            var courseObjForGra = await _courseObjectiveEFRepository.FirstOrDefaultAsync(c => c.GraduationRequirementId == id);
            if (courseObjForGra == null)
            {
                await _graduationRequirementEFRepository.DeleteAsync(id);
                return new DeleteResult();
            }
            return new DeleteResult("该毕业要求已被课程目标绑定，无法删除");
        }
    }
}
