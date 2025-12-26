using AutoMapper;
using EduAdmin.AppService.Courses.Dto;
using EduAdmin.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.CourseObjectives.Dto
{
    public class CourseObjectiveMapFileDto : Profile
    {
        public CourseObjectiveMapFileDto()
        {
            CreateMap<CourseObjDto, CourseObjective>().AfterMap((dto,sourse) =>
            {
                //sourse.ScoreProportion = JsonConvert.SerializeObject(dto.ScoreRate);
            });
            CreateMap<CourseObjective, CourseObjectiveShowDto>().AfterMap((sourse, dto) =>
            {
                //dto.ScoreRate = JsonConvert.DeserializeObject<List<ScoreProportion>>(sourse.ScoreProportion);
            }); 
        }
    }
}
