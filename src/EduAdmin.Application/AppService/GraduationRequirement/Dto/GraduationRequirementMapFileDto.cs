using AutoMapper;
using EduAdmin.AppService.Courses.Dto;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.GraduationRequirements.Dto
{
    public class GraduationRequirementMapFileDto : Profile
    {
        public GraduationRequirementMapFileDto()
        {
            CreateMap<CreateGraduationRequirementDto, GraduationRequirement>();
            CreateMap<GraduationRequirement, GraduationRequirementShowDto>();
        }
    }
}
