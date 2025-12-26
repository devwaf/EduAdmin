using AutoMapper;
using EduAdmin.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Courses.Dto
{
    public class DefenseRecordMapDto : Profile
    {
        public DefenseRecordMapDto()
        {
            CreateMap<CreateDefenseRecordDto, DesignDefenseRecord>();
            CreateMap<UpdateDefenseRecordDto, DesignDefenseRecord>();
            CreateMap<DesignDefenseRecord, DefenseRecordShowDto>().AfterMap((sourse, dto) =>
            {
                dto.WordUrl = sourse.WordUrl;
                dto.Members = JsonConvert.DeserializeObject<List<string>>(sourse.Members);
            });
        }
    }
}
