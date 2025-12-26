using EduAdmin.AppService.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.GraduationRequirements.Dto
{
    public class GraduationRequirementShowDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 要求
        /// </summary>
        public string Require { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        public List<TargetShowDto> Target { get; set; }
    }
}
