using Abp.Authorization;
using Abp.Domain.Repositories;
using AutoMapper;
using EduAdmin.AppService.CourseObjectives.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.CourseObjectives
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class CourseObjectiveAppService:EduAdminAppServiceBase,ICourseObjectiveAppService
    {
        private readonly IRepository<GraduationRequirement, Guid> _graduationRequirementEFRepository;
        private readonly IRepository<CourseObjective, Guid> _courseObjectiveEFRepository;
        private readonly IRepository<Question, Guid> _questuionEFRepository;
        private readonly IRepository<SwDetail, Guid> _swDetailEFRepository;
        private readonly IRepository<Target, Guid> _targetEFRepository;
        private readonly IRepository<ScoreWeight, Guid> _scoreWeightEFRepository;
        public CourseObjectiveAppService(
            IRepository<GraduationRequirement, Guid> graduationRequirementEFRepository,
            IRepository<CourseObjective, Guid> CourseObjectiveEFRepository,
            IRepository<ScoreWeight, Guid> scoreWeightEFRepository,
            IRepository<SwDetail, Guid> swDetailEFRepository,
            IRepository<Question, Guid> questuionEFRepository,
            IRepository<Target, Guid> targetEFRepository)
        {
            _graduationRequirementEFRepository = graduationRequirementEFRepository;
            _courseObjectiveEFRepository = CourseObjectiveEFRepository;
            _scoreWeightEFRepository = scoreWeightEFRepository;
            _questuionEFRepository = questuionEFRepository;
            _targetEFRepository = targetEFRepository;
            _swDetailEFRepository = swDetailEFRepository;
        }
        /// <summary>
        /// 获取所有课程目标
        /// </summary>
        /// <returns></returns>
        public async Task<List<CourseObjectiveShowDto>> GetAllCourseObjective(Guid outlineId)
        {
            List<CourseObjectiveShowDto> list = new List<CourseObjectiveShowDto>();
            var graReq = await _graduationRequirementEFRepository.GetAllListAsync();
            var CourseObjectiveList = await _courseObjectiveEFRepository.GetAllListAsync(c => c.OutlineId == outlineId);
            var target = await _targetEFRepository.GetAllListAsync();
            foreach(var courseObjective in CourseObjectiveList)
            {
                string graName = null;
                var gra = graReq.FirstOrDefault(c => c.Id == courseObjective.GraduationRequirementId);
                if (gra == null)
                {
                    var tar = target.FirstOrDefault(c => c.Id == courseObjective.GraduationRequirementId);
                    gra = graReq.FirstOrDefault(c => c.Id == tar?.GraduationRequireId);
                    graName = gra?.Name + tar?.Name;
                }
                else
                {
                    graName = gra.Name;
                }
                list.Add(new CourseObjectiveShowDto
                {
                    Id = courseObjective.Id,
                    Content = courseObjective.Content,
                    DegreeSupport = courseObjective.DegreeSupport,
                    GraduationRequirement = graName,
                    GraduationRequirementId = courseObjective.GraduationRequirementId,
                    //GredeProportion = courseObjective.GredeProportion,
                    //ScoreRate = JsonConvert.DeserializeObject<List<ScoreProportion>>(courseObjective.ScoreProportion),
                    Name = courseObjective.Name,
                });
            }
            return list;
        }
        /// <summary>
        /// 课程目标下拉框
        /// </summary>
        /// <param name="outlineId"></param>
        /// <returns></returns>
        public async Task<List<SelectDto<Guid>>> GetAllCourseObjectiveSelect(Guid outlineId)
        {
            var list = await GetAllCourseObjective(outlineId);
            return list.Select(c=> new SelectDto<Guid>(c.Id,c.Name)).ToList();
        }
        /// <summary>
        /// 添加课程目标列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> AddCourseObjectiveList(CreateCourseObjectiveDto input)
        {
            foreach(var courseObj in input.CourseObjs)
            {
                var courseObjective = ObjectMapper.Map<CourseObjective>(courseObj);
                courseObjective.OutlineId = input.OutlineId;
                var id = await _courseObjectiveEFRepository.InsertAndGetIdAsync(courseObjective);
            }
            return true;
        }

        //public async Task<AddResult<Guid>> AddCourseObjective(CourseObjDto input)
        //{
        //    var courseObjective = ObjectMapper.Map<CourseObjective>(input);
        //    courseObjective.OutlineId = input.OutlineId;
        //    var id = await _courseObjectiveEFRepository.InsertAndGetIdAsync(courseObjective);
        //    return new AddResult<Guid>(id);
        //}

        /// <summary>
        /// 添加课程目标
        /// </summary>
        /// <param name="outlineId"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddCourseObjective(Guid outlineId)
        {
           var id = await _courseObjectiveEFRepository.InsertAndGetIdAsync(new CourseObjective
            {
                OutlineId = outlineId
            });
            return new AddResult<Guid>(id);
        }

        /// <summary>
        /// 修改课程目标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateCourseObjective(CourseObjDto input)
        {
            var courseObj =  await _courseObjectiveEFRepository.GetAsync(input.Id);
            courseObj.Content = input.Content;
            courseObj.Name = input.Name;
            courseObj.GraduationRequirementId = input.GraduationRequirementId;
            courseObj.DegreeSupport = input.DegreeSupport;
            return new UpdateResult();
        }
        /// <summary>
        /// 删除课程目标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteCourseObjective(Guid id)
        {
            var question = await _questuionEFRepository.FirstOrDefaultAsync(c => c.CourseObjectiveId == id);
            if(question != null)
            {
                return new DeleteResult("课程目标已被小题绑定不能删除");
            }
            var swDetail = await _swDetailEFRepository.FirstOrDefaultAsync(c => c.CourseObjectiveId == id);
            if(swDetail != null)
            {
                return new DeleteResult("课程目标已被权重详细绑定不能删除");
            }
            await _courseObjectiveEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
        /// <summary>
        /// 获取课程目标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CourseObjectiveShowDto> GetCourse(Guid id)
        {
            var courseObjective = await _courseObjectiveEFRepository.GetAsync(id);
            return ObjectMapper.Map<CourseObjectiveShowDto>(courseObjective);
        }
        
    }
}
