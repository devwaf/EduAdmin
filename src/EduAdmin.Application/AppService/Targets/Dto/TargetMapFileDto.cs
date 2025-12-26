using AutoMapper;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Targets.Dto
{
    public class TargetMapFileDto : Profile
    {
        public TargetMapFileDto()
        {
            CreateMap<CreateTargetDto, Target>();
            CreateMap<Target, TargetShowDto>();
        }
    }
}
