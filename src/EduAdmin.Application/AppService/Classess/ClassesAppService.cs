using Abp.Authorization;
using Abp.Domain.Repositories;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Classess
{
    /// <summary>
    /// 教师
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class ClassesAppService : EduAdminAppServiceBase, IClassesAppService
    {
        private readonly IRepository<Classes, Guid> _classesEFRepository;
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IRepository<ClassCourse, Guid> _classCourseEFRepository;
        public ClassesAppService(
           IRepository<Classes, Guid> classesEFRepository,
           IRepository<Student, Guid> studentEFRepository,
           IRepository<ClassCourse, Guid> classCourseEFRepository
           )
        {
            _classesEFRepository = classesEFRepository;
            _studentEFRepository = studentEFRepository;
            _classCourseEFRepository = classCourseEFRepository;
        }
        /// <summary>
        /// 获取所有班级
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]//不需登录即可调用
        public async Task<List<ClassesShowDto>> GetAllClasses()
        {
            var classesList = await _classesEFRepository.GetAllListAsync();
            return ObjectMapper.Map<List<ClassesShowDto>>(classesList);
        }
        /// <summary>
        /// 获取班级下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectDto<Guid>>> GetAllClassSelect()
        {
            var result = await _classesEFRepository.GetAll().OrderBy(c => c.Name).Select(c => new SelectDto<Guid>(c.Id, c.SchoolYear + c.Major + c.Name)).ToListAsync();
            return result;
        }
        /// <summary>
        /// 获取课程班级下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectDto<Guid>>> GetAllCourseClassSelect(Guid courseId)
        {
            var classIds = await _classCourseEFRepository.GetAll().Where(c=>c.CourseId == courseId).Select(C=>C.ClassId).ToListAsync();
            var result = await _classesEFRepository.GetAll().Where(c=>classIds.Contains(c.Id)).OrderBy(c=>c.Name).Select(c => new SelectDto<Guid>(c.Id, c.SchoolYear + c.Major + c.Name)).ToListAsync();
            return result;
        }
        /// <summary>
        /// 添加班级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddClasses(CreateClassesDto input)
        {
            var classes = ObjectMapper.Map<Classes>(input);
            var id = await _classesEFRepository.InsertAndGetIdAsync(classes);
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改班级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateClasses(CreateClassesDto input)
        {
            var classes = ObjectMapper.Map<Classes>(input);
            await _classesEFRepository.UpdateAsync(classes);
            return new UpdateResult();
        }
        /// <summary>
        /// 获取一个班级内所有的学生
        /// </summary>
        /// <returns></returns>
        public async Task<List<Guid>> GetClassAllStu(Guid classId)
        {
            var stuIds = await _studentEFRepository.GetAll().Where(c => c.ClassId == classId).Select(c => c.Id).ToListAsync();
            return stuIds;
        }
        /// <summary>
        /// 删除班级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteClasses(Guid id)
        {
            var res = await GetClassAllStu(id);
            if(res.Count() == 0)
            {
                await _classesEFRepository.DeleteAsync(id);
                return new DeleteResult();
            }
            return new DeleteResult("班级下有学生，不可以删除");
        }
    }
}
