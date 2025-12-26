using AutoMapper;
using EduAdmin.Entities;
using EduAdmin.LocalTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Outlines.Dto
{
    public class OutlineMapFileDto:Profile
    {
        public OutlineMapFileDto()
        {
                CreateMap<OutlineCreateDto, Outline>();
                CreateMap<Question, OutlineShowDto>();
                CreateMap<Outline, OutlineShowDto>().AfterMap((sourse, dto) =>
                {
                    dto.CreationTime = LocalTool.TimeFormatStr(sourse.CreationTime,2);
                });    
        }

    }
}
