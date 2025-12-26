using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Entities
{
    [Table("QuestionScore")]
    public class QuestionScore : Entity<Guid>
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public Guid StudentId  { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// 小题名称
        /// </summary>
        public string TitleNum { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public double Score { get; set; }
    }
}
