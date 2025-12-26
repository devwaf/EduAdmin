using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Courses.Dto
{
    public class UpdateDefenseRecordDto
    {

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid Id { get; set; }


        /// <summary>
        /// 答辩成绩
        /// </summary>
        public virtual float DefenseScore { get; set; }
        /// <summary>
        /// 答辩组长
        /// </summary>
      //  public virtual string GroupLeader { get; set; }

        /// <summary>
        /// 答辩意见
        /// </summary>
        public virtual string DefenseOpinion { get; set; }

    }
}
