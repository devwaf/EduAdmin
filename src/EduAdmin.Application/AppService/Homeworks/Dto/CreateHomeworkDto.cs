using EduAdmin.FileManagements.Dto;
using System;
using System.Collections.Generic;

namespace EduAdmin.AppService.Homeworks
{
    public class CreateHomeworkDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 班级ID
        /// </summary>
        //public virtual List<Guid> ClassesId { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual string FileType { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public virtual Guid CourseId { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        //public virtual int? Score { get; set; }
        /// <summary>
        /// 作业次数
        /// </summary>
        public virtual int Times { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public virtual DateTime? ClosingDate { get; set; }
        /// <summary>
        /// 是否发送消息
        /// </summary>
        public virtual bool IsNotSendMessage { get; set; }
    }
    public class UpdateTHomeworkDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid id { get; set;}
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 班级ID
        /// </summary>
        public virtual Guid  ClassesId { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual string FileType { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public virtual Guid CourseId { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual int? Score { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public virtual DateTime? ClosingDate { get; set; }
    }
    public class SubmitStuHomeworkDto
    {
        /// <summary>
        /// 学生作业ID
        /// </summary>
        public Guid StuHomeworkId { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public FileDto FileList { get; set; }
    }
}