using EduAdmin.AppService.Questions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Questions
{
    public interface IQuestionAppService
    {
        /// <summary>
        /// 获取所有小题
        /// </summary>
        /// <param name="outlineId"></param>
        /// <returns></returns>
        Task<List<QuestionShowDto>> GetAllQuestion(Guid outlineId);
    }
}
