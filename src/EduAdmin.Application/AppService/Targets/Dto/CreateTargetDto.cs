using System;

namespace EduAdmin.AppService.Targets
{
    public class CreateTargetDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 毕业要求Id
        /// </summary>
        public virtual string GraduationRequireId { get; set; }
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