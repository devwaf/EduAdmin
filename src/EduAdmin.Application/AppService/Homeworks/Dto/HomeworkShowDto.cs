using EduAdmin.FileManagements.Dto;
using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;

namespace EduAdmin.AppService.Homeworks
{
    public class HomeworkShowDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual int? Score { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public virtual string ClosingDate { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public virtual string CourseName { get; set; }
    }
    public class StudentHomeworkAndRate
    {
        /// <summary>
        /// 比率
        /// </summary>
        public string Rate { get; set; }
        /// <summary>
        /// 学生作业
        /// </summary>
        public List<StudentHomeworkShowDto> StudentHomeworks { get; set; }
    }
    public class StudentHomeworkShowDto
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public Guid StuId { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public virtual string ClassName { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public virtual string Sno { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string StuName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool Gender { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public virtual string CourseName { get; set; }
        /// <summary>
        /// 学生的作业
        /// </summary>
        public virtual List<StuHomework> StuHomeworks { get; set; }
    }
    public class StuHomework
    {
        public StuHomework()
        {
        }
        public StuHomework(string homeworkName, int? homeworkScore, bool state)
        {

            HomeworkName = homeworkName;
            HomeworkScore = homeworkScore;
            State = state;
        }
        /// <summary>
        /// 学生作业ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public string HomeworkName { get; set; }
        /// <summary>
        /// 作业分数
        /// </summary>
        public double? HomeworkScore { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool? State { get; set; }
        public Guid HomeworkId { get; set; }
    }
    public class StuHomeworkAndCountRateDto
    {
        /// <summary>
        /// 完成情况的比
        /// </summary>
        public string Rate { get; set; }
        /// <summary>
        /// 学生的所有作业
        /// </summary>
        public List<StudentAllHomeworkShowDto> StuAllHomework { get; set; }
    }
    public class StudentAllHomeworkShowDto
    {
        /// <summary>
        /// 学生作业Id
        /// </summary>
        public Guid StuHomeworkId { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public string HomeworkName { get; set; }
        /// <summary>
        /// 作业文件
        /// </summary>
        public FileDto FilePath { get; set; }
        /// <summary>
        /// 作业文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool? State { get; set; }
    }
    public class StudentHomeworkHistory
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string HomeworkName { get; set;}
        /// <summary>
        /// 分数
        /// </summary>
        public double? Score { get; set; }
        /// <summary>
        /// 评语
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 批改时间
        /// </summary>
        public string UpdateTime { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string  CourseName { get; set; }
    }
}