using AutoMapper;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.AchievementTargets.Dto
{
    public class AchievementTargetMapFileDto:Profile
    {
        public AchievementTargetMapFileDto()
        {
            CreateMap<CreateAchievementTargetDto, AchievementTarget>();
            CreateMap<AchievementTarget, AchievementTargetShowDto>();
        }
    }
}
