using Abp.Authorization;
using Abp.Domain.Repositories;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Targets
{
    /// <summary>
    /// 教师
    /// </summary>
    [AbpAuthorize]
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class TargetAppService : EduAdminAppServiceBase, ITargetAppService
    {
        private readonly IRepository<Target, Guid> _targetEFRepository;
        public TargetAppService(
           IRepository<Target, Guid> TargetEFRepository)
        {
            _targetEFRepository = TargetEFRepository;
        }
        /// <summary>
        /// 添加毕业要求的目标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AddResult<Guid>> AddTarget(CreateTargetDto input)
        {
            var target = ObjectMapper.Map<Target>(input);
            var id = await _targetEFRepository.InsertAndGetIdAsync(target);
            return new AddResult<Guid>(id);
        }
        /// <summary>
        /// 修改毕业要求的目标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateTarget(CreateTargetDto input)
        {
            var target = ObjectMapper.Map<Target>(input);
            await _targetEFRepository.UpdateAsync(target);
            return new UpdateResult();
        }
        /// <summary>
        /// 删除毕业要求的目标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteTarget(Guid id)
        {
            await _targetEFRepository.DeleteAsync(id);
            return new DeleteResult();
        }
    }
}
