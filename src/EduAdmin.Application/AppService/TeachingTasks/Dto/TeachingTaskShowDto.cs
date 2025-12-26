using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.TeachingTasks.Dto
{
    public class TeachingTaskShowDto
    {
        /// <summary>
        /// 对应序号的填充数据
        /// </summary>
        public virtual TeaTaskContent NumS1 { get; set; }
        public virtual TeaTaskContent NumS2 { get; set; }
        public virtual TeaTaskContent NumS3 { get; set; }
        public virtual TeaTaskContent NumS4 { get; set; }
        public virtual TeaTaskContent NumS5 { get; set; }
        public virtual TeaTaskContent NumS6 { get; set; }
        public virtual TeaTaskContent NumS7 { get; set; }
        public virtual TeaTaskContent NumS8 { get; set; }
        public virtual TeaTaskContent NumS9 { get; set; }
        public virtual TeaTaskContent NumS10 { get; set; }
        public virtual TeaTaskContent NumS11 { get; set; }
        public virtual TeaTaskContent NumS12 { get; set; }
        public virtual TeaTaskContent NumS13 { get; set; }
        public virtual TeaTaskContent NumS14 { get; set; }
        public virtual TeaTaskContent NumS15 { get; set; }
        public virtual TeaTaskContent NumS16 { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public virtual string FilePath { get; set; }
    }
}
