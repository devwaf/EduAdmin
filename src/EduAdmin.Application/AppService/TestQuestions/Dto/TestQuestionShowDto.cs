using EduAdmin.AppService.Questions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.TestQuestions.Dto
{
    public class TestQuestionShowDto
    {
        /// <summary>
        /// 大题Id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 题号
        /// </summary>
        public virtual string TitleNum { get; set; }
        /// <summary>
        /// 课程目标名称
        /// </summary>
        public virtual string CourseObjName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual int? Score { get; set; }
        /// <summary>
        /// 课程目标Id
        /// </summary>
        public Guid? CourseObjectiveId { get; set; }
        /// <summary>
        /// 小题
        /// </summary>
        public List<QuestionShowDto> Question { get; set; }
    }
    public class TestQueAccount
    {
        /// <summary>
        /// 小题总数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 所有小题的分数之和
        /// </summary>
        public int ScoreNum { get; set; }
        /// <summary>
        /// 所有大题包含小题
        /// </summary>
        public List<TestQuestionShowDto> TestQuestions { get; set; }
    }
}
