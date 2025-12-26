using AutoMapper;
using EduAdmin.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.TeachingTasks.Dto
{
    public class TeachingTaskMapFile : Profile
    {
        public TeachingTaskMapFile()
        {
            CreateMap<AddTeachingTaskDto, TeachingTask>().AfterMap((dto, sourse) =>
            {
                sourse.Num1 = JsonConvert.SerializeObject(dto.Content.Num1);
                sourse.Num2 = JsonConvert.SerializeObject(dto.Content.Num2);
                sourse.Num3 = JsonConvert.SerializeObject(dto.Content.Num3);
                sourse.Num4 = JsonConvert.SerializeObject(dto.Content.Num4);
                sourse.Num5 = JsonConvert.SerializeObject(dto.Content.Num5);
                sourse.Num6 = JsonConvert.SerializeObject(dto.Content.Num6);
                sourse.Num7 = JsonConvert.SerializeObject(dto.Content.Num7);
                sourse.Num8 = JsonConvert.SerializeObject(dto.Content.Num8);
                sourse.Num9 = JsonConvert.SerializeObject(dto.Content.Num9);
                sourse.Num10 = JsonConvert.SerializeObject(dto.Content.Num10);
                sourse.Num11 = JsonConvert.SerializeObject(dto.Content.Num11);
                sourse.Num12 = JsonConvert.SerializeObject(dto.Content.Num12);
                sourse.Num13 = JsonConvert.SerializeObject(dto.Content.Num13);
                sourse.Num14 = JsonConvert.SerializeObject(dto.Content.Num14);
                sourse.Num15 = JsonConvert.SerializeObject(dto.Content.Num15);
                sourse.Num16 = JsonConvert.SerializeObject(dto.Content.Num16);
            });
            CreateMap<TeachingTask, TeachingTaskShowDto>().AfterMap((sourse, dto) =>
            {
                dto.NumS1 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num1);
                dto.NumS2 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num2);
                dto.NumS3 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num3);
                dto.NumS4 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num4);
                dto.NumS5 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num5);
                dto.NumS6 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num6);
                dto.NumS7 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num7);
                dto.NumS8 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num8);
                dto.NumS9 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num9);
                dto.NumS10 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num10);
                dto.NumS11 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num11);
                dto.NumS12 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num12);
                dto.NumS13 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num13);
                dto.NumS14 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num14);
                dto.NumS15 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num15);
                dto.NumS16 = JsonConvert.DeserializeObject<TeaTaskContent>(sourse.Num16);
            });
        }
    }
}
