using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreWeights.Dto
{
    public class UpdateSwDetailDto
    {
        /// <summary>
        /// 作业详细Id
        /// </summary>
       public Guid Id { get; set; }
        /// <summary>
        /// 课程目标Id
        /// </summary>
        public Guid? CourseObjectiveId { get; set; }
    }
}
