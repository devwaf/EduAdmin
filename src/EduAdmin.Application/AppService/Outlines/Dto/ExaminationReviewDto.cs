using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Outlines.Dto
{
    public class ExaminationReviewDto
    {
        /// <summary>
        /// 按照word从上到下
        /// </summary>
        public Guid CourseId { get; set; }
        public DateTime  ExamTime { get; set; }
        public List<ExamTableDto> ExamTable1 { get; set; }
        public List<ExamTableDto> ExamTable2 { get; set; }
        public int WingdingsExam { get; set; }
        public int WingdingsExceed { get; set; }
        public int WingdingsCover { get; set; }
        public int WingdingsModelStyle { get; set; }
        public int WingdingsContent { get; set; }
        public int WingdingsClear { get; set; }
        public int WingdingsHundred { get; set; }
        public int WingdingsTypeSuit { get; set; }
        public int WingdingsNumSuit { get; set; }
        public int WingdingsExceedS { get; set; }
        public int WingdingsError { get; set; }
        public int WingdingsAgree { get; set; }
        public int WingdingsAgreeS { get; set; }
        public int WingdingsIsA { get; set; }
        public int WingdingsIsAS { get; set; }
    }
    public class ExamTableDto
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
        /// 是否一致
        /// </summary>
        public int IsPass { get; set; }
    }
}
