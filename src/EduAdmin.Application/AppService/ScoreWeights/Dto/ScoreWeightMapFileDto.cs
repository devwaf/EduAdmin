using AutoMapper;
using EduAdmin.AppService.Courses.Dto;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreWeights.Dto
{
    public class ScoreWeightMapFileDto : Profile
    {
        public ScoreWeightMapFileDto()
        {
            CreateMap<CreateScoreWeightDto, ScoreWeight>();
            CreateMap<UpdateScoreWeight, ScoreWeight>();
            CreateMap<ScoreWeight, ScoreWeightShowDto>();
        }
    }
}
