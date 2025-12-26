using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.DefenseRecord.Dto
{
    public  class EcportAchiveInputDto
    {
        public Guid CourseId { get; set; }
        public Guid? ClassId { get; set; }
        public string TeacherName { get; set; }
        /// <summary>
        /// 考试时间
        /// </summary>
        public DateTime ExamTime { get; set; }
        /// <summary>
        /// 实考人数
        /// </summary>
        public string TrueExamPeople { get; set; }
        /// <summary>
        /// 整体达成评价结果分析
        /// </summary>
        public string AnalEvalua { get; set; }
        /// <summary>
        /// 题目个数
        /// </summary>
        public int QuestionCount { get; set; }
        /// <summary>
        ///学生个体达成分析
        /// </summary>
        public string StudentAnalEvalua { get; set; }
        /// <summary>
        /// 存在问题或不足
        /// </summary>
        public string Problem1 { get; set; }
        /// <summary>
        /// 评估结果
        /// </summary>
        public string Assess { get; set; }
        /// <summary>
        /// 存在问题
        /// </summary>
        public string Problem2 { get; set; }
        /// <summary>
        /// 后期改进措施
        /// </summary>
        public string Improve { get; set; }
        /// <summary>
        /// 评价方式
        /// </summary>
        public int EvaluationMethod { get; set; }
        /// <summary>
        /// 是否符合大纲要求
        /// </summary>
        public int FillBill { get; set; }
        /// <summary>
        /// 试卷难易程度
        /// </summary>
        public int TestDifficulty { get; set; }
    }
}
