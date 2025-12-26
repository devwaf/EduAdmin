using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Outlines.Dto
{
    public class OutlineShowDto
    {
        /// <summary>
        /// 大纲Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 大纲名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 大纲类型（课程，课设）
        /// </summary>
        public virtual string Kind { get; set; }
        /// <summary>
        /// 是否完整
        /// </summary>
        public virtual bool IsComplete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
    }
    public class AllTableDto
    {
        public List<Dictionary<string, string>> DicsList { get; set; }
        public List<SelectDto<string>> List { get; set; }
    }
}
