using System;

namespace EduAdmin.AppService.GraduationDesigns
{
    public class GraduationDesignShowDto
    {
        /// <summary>
        /// 毕业设计Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public Guid StuId { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StuName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string Sno { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string Classses { get; set; }
        /// <summary>
        /// 毕业设计名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 毕业设计状态
        /// </summary>
        public virtual bool? State { get; set; }
        /// <summary>
        /// 任务书
        /// </summary>
        public virtual ShowGraFileDto Assignment { get; set; }
        /// <summary>
        /// 开题报告
        /// </summary>
        public virtual ShowGraFileDto Headline { get; set; }
        /// <summary>
        /// 外文翻译
        /// </summary>
        public virtual ShowGraFileDto ForeignTrans { get; set; }
        /// <summary>
        /// 论文草稿
        /// </summary>
        public virtual ShowGraFileDto DraftDissertation { get; set; }
        /// <summary>
        /// 第一阶段情况报告
        /// </summary>
        public virtual ShowGraFileDto FirstReport { get; set; }
        /// <summary>
        /// 第二阶段情况报告
        /// </summary>
        public virtual ShowGraFileDto SecondReport { get; set; }
        /// <summary>
        /// 论文
        /// </summary>
        public virtual ShowGraFileDto Dissertation { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public virtual ShowGraFileDto Annex { get; set; }
        /// <summary>
        /// 查重报告
        /// </summary>
        public virtual ShowGraFileDto CheckReport { get; set; }
    }
    public class GraduationDesignStuShowDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 毕业设计名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 毕业设计状态
        /// </summary>
        public virtual bool? State { get; set; }
        /// <summary>
        /// 任务书
        /// </summary>
        public virtual ShowGraFileDto Assignment { get; set; }
        /// <summary>
        /// 开题报告
        /// </summary>
        public virtual ShowGraFileDto Headline { get; set; }
        /// <summary>
        /// 外文翻译
        /// </summary>
        public virtual ShowGraFileDto ForeignTrans { get; set; }
        /// <summary>
        /// 论文草稿
        /// </summary>
        public virtual ShowGraFileDto DraftDissertation { get; set; }
        /// <summary>
        /// 第一阶段情况报告
        /// </summary>
        public virtual ShowGraFileDto FirstReport { get; set; }
        /// <summary>
        /// 第二阶段情况报告
        /// </summary>
        public virtual ShowGraFileDto SecondReport { get; set; }
        /// <summary>
        /// 论文
        /// </summary>
        public virtual ShowGraFileDto Dissertation { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public virtual ShowGraFileDto Annex { get; set; }
        /// <summary>
        /// 查重报告
        /// </summary>
        public virtual ShowGraFileDto CheckReport { get; set; }
        /// <summary>
        /// 是否开始
        /// </summary>
        public bool IsStart { get; set; }
    }
    public class ShowGraFileDto
    { 
        /// <summary>
        /// 状态
        /// </summary>
        public bool? State { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Url { get; set; }
    }

}