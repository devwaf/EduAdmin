using AutoMapper;
using EduAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Courses.Dto
{
    public class CourseMapFileDto:Profile
    {
        public CourseMapFileDto()
        {
            CreateMap<CreateCourseDto, Course>();
            CreateMap<Course, CourseShowDto>();
        }
    }
}
