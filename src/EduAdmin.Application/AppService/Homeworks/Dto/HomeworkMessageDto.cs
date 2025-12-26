using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Homeworks.Dto
{
    public class HomeworkMessageDto
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public Guid NoticeId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Router { get; set; }
    }
    public class HomeworkTeaMessageDto
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 个数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Router { get; set; }
    }
}
