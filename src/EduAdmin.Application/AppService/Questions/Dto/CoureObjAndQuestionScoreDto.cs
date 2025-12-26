using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.CourseObjectives.Dto
{
    public class CoureObjAndQuestionScoreDto
    {
        /// <summary>
        /// 课程目标内容
        /// </summary>
        public string ObjContent { get; set; }
        /// <summary>
        /// 试题对应分值
        /// </summary>
        public List<string> QuestionScore { get; set; }
        /// <summary>
        /// 是或否
        /// </summary>
        public int IsPass { get; set; } = 1;
    }
}
