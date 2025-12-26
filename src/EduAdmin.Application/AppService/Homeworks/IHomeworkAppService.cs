using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Homeworks
{
    public interface IHomeworkAppService
    {
        /// <summary>
        /// 老师发布作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResultDto> AddHomework(CreateHomeworkDto input);
    }
}
