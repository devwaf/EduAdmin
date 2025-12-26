using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreWeights.Dto
{
    public class ScoreWeightShowDto
    {
        /// <summary>
        /// 权重Id
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// 权重名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 权重占比
        /// </summary>
        public virtual int Power { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public int Times { get; set; }
    }
    public class SwDetailShowDto
    {
        /// <summary>
        /// 权重名称
        /// </summary>
        public string ScoreWeightName { get; set; }
        /// <summary>
        /// 权重详细
        /// </summary>
        public List<SwDetailList> SwDetails { get; set; }
    }
    public class SwDetailList
    {
        /// <summary>
        /// 作业Id
        /// </summary>
        public Guid SwDetailId  { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public string SwDetailName { get; set; }
        /// <summary>
        /// 作业权重
        /// </summary>
        public float? SwDetailPower { get; set; }
        /// <summary>
        /// 对应课程目标Id
        /// </summary>
        public Guid? CourseObjectiveId   { get; set; }
    }
}
