using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Outlines.Dto
{
    public class CourObjCreateTableDto
    {
        /// <summary>
        /// 课程目标内容
        /// </summary>
        public string CourObjContent { get; set; }
        /// <summary>
        /// 目标达成值
        /// </summary>
        public string AllAchiveScore { get; set; }
        /// <summary>
        /// 表格多条部分
        /// </summary>
        public List<TableSplitDto> TableSplit { get; set; }
        /// <summary>
        /// 个数
        /// </summary>
        public int Count { get; set; }
        
    }
    public class TableSplitDto
    {
        /// <summary>
        /// 考试考核及占比
        /// </summary>
        public string Rate { get; set; }
        /// <summary>
        /// 考试考核目标分支
        /// </summary>
        public string ObjScore { get; set; }
        /// <summary>
        /// 考试考核平均分值
        /// </summary>
        public string AvgScore { get; set; }
        /// <summary>
        /// 考试考核目标达成值
        /// </summary>
        public string AchiveScore { get; set; }
    }
    public class CourObjCreateTableForMajor
    {
        public string CourObjContent { get; set; }
        public List<ClassAchiveDto> ClassAchive { get; set; }
    }
    public class ClassAchiveDto
    {
        public string ClassName { get; set; }
        public string AchiveScore { get; set; }
    }
}
