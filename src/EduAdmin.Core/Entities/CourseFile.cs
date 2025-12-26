using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.Entities
{
    [Table("CourseFile")]
    public class CourseFile: Entity<Guid>
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// 课程报告
        /// </summary>
        public string CourseReport { get; set; }
        /// <summary>
        /// 任务书
        /// </summary>
        public string TaskBook { get; set; }
    }
}
