using AutoMapper;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.TestQuestions.Dto
{
    public class TestQuestionMapFileDto:Profile
    {
        public TestQuestionMapFileDto()
        {
            CreateMap<CreateTestQuestionDto, TestQuestion>();
            CreateMap<TestQuestion, TestQuestionShowDto>();
        }
    }
}
