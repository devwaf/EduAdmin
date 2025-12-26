using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Homeworks.Dto
{
    public class MessageDto : NotificationData
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Router { get; set; }
        /// <summary>
        /// 作业或毕业设计Id
        /// </summary>
        public Guid WorkId { get; set; }

        public MessageDto(string content, string creationTime, string state, string router, Guid workId)
        {

            Content = content;
            CreationTime = creationTime;
            State = state;
            Router = router;
            WorkId = workId;
        }
    }
}
