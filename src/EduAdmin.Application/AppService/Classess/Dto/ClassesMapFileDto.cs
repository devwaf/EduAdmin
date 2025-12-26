using AutoMapper;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Classess.Dto
{
    public class ClassesMapFileDto : Profile
    {
        public ClassesMapFileDto()
        {
            CreateMap<CreateClassesDto, Classes>();
            CreateMap<Classes, ClassesShowDto>();
        }
    }
}
