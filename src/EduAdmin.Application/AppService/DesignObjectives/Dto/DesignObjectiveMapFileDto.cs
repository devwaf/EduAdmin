using AutoMapper;
using EduAdmin.AppService.Courses.Dto;
using EduAdmin.AppService.ScoreAchievements.Dto;
using EduAdmin.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.DesignObjectives.Dto
{
    public class DesignObjectiveMapFileDto : Profile
    {
        public DesignObjectiveMapFileDto()
        {
            CreateMap<CreateDesignObj, DesignObjective>().AfterMap((dto,sourse) =>
            {
                sourse.ScoreProportion = JsonConvert.SerializeObject(dto.ScoreRate);
            });
            CreateMap<DesignObjective, DesignObjectiveShowDto>().AfterMap((sourse, dto) =>
            {
                dto.ScoreRate = JsonConvert.DeserializeObject<List<ScoreAchievementShowDto>>(sourse.ScoreProportion);
            }); 
        }
    }
}
