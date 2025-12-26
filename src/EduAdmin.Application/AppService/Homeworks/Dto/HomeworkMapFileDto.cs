using AutoMapper;
using EduAdmin.Entities;
using EduAdmin.LocalTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Homeworks.Dto
{
    public class HomeworkMapFileDto : Profile
    {
        public HomeworkMapFileDto()
        {
            CreateMap<CreateHomeworkDto, Homework>();
            CreateMap<Homework, HomeworkShowDto>().AfterMap((sourse, dto) =>
            {
                if (sourse.ClosingDate != null)
                    dto.ClosingDate = LocalTool.TimeFormatStr((DateTime)sourse.ClosingDate, 6);
                dto.CreationTime = LocalTool.TimeFormatStr(sourse.CreationTime, 6);
            });
        }
    }
}
