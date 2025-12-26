using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Outlines.Dto
{
    public class OutlineCreateDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 大纲名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 教师Id
        /// </summary>
        public virtual Guid TeacherId { get; set; }
        /// <summary>
        /// 大纲类型（课程，课设）
        /// </summary>
        public virtual string Kind { get; set; }

    }
}
