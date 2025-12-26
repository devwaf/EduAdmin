using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Entities
{
    [Table("DesignDefenseRecord")]
    public class DesignDefenseRecord : Entity<Guid>
    {


        /// <summary>
        /// 学院名称
        /// </summary>
        public virtual string CollegeName { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public virtual string Major { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public virtual string StudentName { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public virtual string Sno { get; set; }
        /// <summary>
        /// 指导老师
        /// </summary>
        public virtual string AcademicAdvisor { get; set; }
        /// <summary>
        /// 题目
        /// </summary>
        public virtual string Subject { get; set; }
        /// <summary>
        /// 答辩成员
        /// </summary>
        public virtual string Members { get; set; }
        /// <summary>
        /// 工作描述
        /// </summary>
        public virtual string JobDescription { get; set; }

        /// <summary>
        /// 问题1
        /// </summary>
        public virtual string QuestionOne { get; set; }
        /// <summary>
        /// 问题2
        /// </summary>
        public virtual string QuestionTwo { get; set; }
        /// <summary>
        /// 问题3
        /// </summary>
        public virtual string QuestionThree { get; set; }

        /// <summary>
        /// 回答1
        /// </summary>
        public virtual string AnswerOne { get; set; }
        /// <summary>
        /// 回答2
        /// </summary>
        public virtual string AnswerTwo { get; set; }
        /// <summary>
        /// 回答3
        /// </summary>
        public virtual string AnswerThree { get; set; }
        /// <summary>
        /// 答辩成绩
        /// </summary>
        public virtual float DefenseScore { get; set; }
        /// <summary>
        /// 答辩组长
        /// </summary>
        public virtual string GroupLeader { get; set; }
        /// <summary>
        /// 答辩时间
        /// </summary>
        public virtual DateTime? DefenseTime { get; set; }
        /// <summary>
        /// 答辩意见
        /// </summary>
        public virtual string DefenseOpinion { get; set; }
        /// <summary>
        /// 课设Id
        /// </summary>
        public virtual Guid CourseDesignId { get; set; }
        /// <summary>
        /// 学生ID
        /// </summary>
        public virtual Guid StudentId { get; set; }
        /// <summary>
        /// 答辩状态
        /// </summary>
        public virtual int State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
        /// <summary>
        /// 答辩文件
        /// </summary>
        public virtual string WordUrl { get; set; }


    }
}
