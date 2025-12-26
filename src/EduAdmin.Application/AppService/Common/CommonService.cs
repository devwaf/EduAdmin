using Abp.Domain.Repositories;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Common
{
    public class CommonService
    {
        private readonly IRepository<Teacher, Guid> _teacherEFRepository;
        public CommonService(IRepository<Teacher, Guid> teacherEFRepository)
        {
            _teacherEFRepository = teacherEFRepository;
        }
        /// <summary>
        /// 获取Guid
        /// </summary>
        /// <returns></returns>
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}
