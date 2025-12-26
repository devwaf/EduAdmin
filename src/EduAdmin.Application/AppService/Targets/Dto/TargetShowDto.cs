using System;

namespace EduAdmin.AppService.Targets
{
    public class TargetShowDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 指标点名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 指标内容
        /// </summary>
        public virtual string Content { get; set; }
    }
}