using AutoMapper;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Questions.Dto
{
    public class QuestionMapFileDto:Profile
    {
        public QuestionMapFileDto()
        {
            CreateMap<CreateQuestionDto, Question>();
            CreateMap<Question, QuestionShowDto>();
        }
    }
}
