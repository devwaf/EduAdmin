using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Questions.Dto
{
    public class CreateQuestionDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 大题Id
        /// </summary>
        public virtual Guid TestQuestionId { get; set; }
        /// <summary>
        /// 题号
        /// </summary>
        public virtual string TitleNum { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual int? Score { get; set; }
        /// <summary>
        /// 课程目标
        /// </summary>
        public virtual Guid? CourseObjectiveId { get; set; }
    }
}
