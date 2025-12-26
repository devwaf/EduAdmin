using AutoMapper;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreAchievements.Dto
{
    public class ScoreAchievementMapFileDto:Profile
    {
        public ScoreAchievementMapFileDto()
        {
            CreateMap<CreateScoreAchievementDto, ScoreAchievement>();
            CreateMap<ScoreAchievement, ScoreAchievementShowDto>();
        }
    }
}
