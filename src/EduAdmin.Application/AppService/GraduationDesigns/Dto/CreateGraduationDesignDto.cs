using EduAdmin.FileManagements.Dto;
using System;

namespace EduAdmin.AppService.GraduationDesigns
{
    public class CreateGraduationDesignDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public Guid StuId { get; set; }
        /// <summary>
        /// 毕业设计名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 任务书
        /// </summary>
        public virtual FileDto Assignment { get; set; }
        /// <summary>
        /// 开题报告
        /// </summary>
        public virtual FileDto Headline { get; set; }
        /// <summary>
        /// 外文翻译
        /// </summary>
        public virtual FileDto ForeignTrans { get; set; }
        /// <summary>
        /// 论文草稿
        /// </summary>
        public virtual FileDto DraftDissertation { get; set; }
        /// <summary>
        /// 第一阶段情况报告
        /// </summary>
        public virtual FileDto FirstReport { get; set; }
        /// <summary>
        /// 第二阶段情况报告
        /// </summary>
        public virtual FileDto SecondReport { get; set; }
        /// <summary>
        /// 论文
        /// </summary>
        public virtual FileDto Dissertation { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public virtual FileDto Annex { get; set; }
        /// <summary>
        /// 查重报告
        /// </summary>
        public virtual FileDto CheckReport { get; set; }
    }
    public class GraDsignFileAndState
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool? State { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public FileDto FilePath { get; set; }
    }
}