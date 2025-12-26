using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Homeworks.Dto
{
    public class UpdateHomeworkDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 评语
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public virtual double? Score { get; set; }
    }
}
