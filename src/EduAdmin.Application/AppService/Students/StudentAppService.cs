using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using EduAdmin.AppService.Students.Dto;
using EduAdmin.Authorization;
using EduAdmin.Entities;
using EduAdmin.LocalTools.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Students
{
    [AbpAuthorize]
    public class StudentAppService : EduAdminAppServiceBase, IStudentAppService
    {
        private readonly IRepository<Student, Guid> _studentEFRepository;
        private readonly IRepository<ClassCourse, Guid> _classCourseEFRepository;
        private readonly IRepository<StudentCourse, Guid> _studentCourseEFRepository;
        private readonly IRepository<Course, Guid> _courseEFRepository;
        private readonly IRepository<Classes, Guid> _classesEFRepository;

        public StudentAppService(IRepository<Student, Guid> studentEFRepository,
            IRepository<ClassCourse, Guid> classCourseEFRepository,
            IRepository<Course, Guid> courseEFRepository,
            IRepository<Classes, Guid> classesEFRepository,
            IRepository<StudentCourse, Guid> studentCourseEFRepository)
        {
            _studentEFRepository = studentEFRepository;
            _classCourseEFRepository = classCourseEFRepository;
            _courseEFRepository = courseEFRepository;
            _classesEFRepository = classesEFRepository;
            _studentCourseEFRepository = studentCourseEFRepository;
        }
        /// <summary>
        /// 获取学生列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sno"></param>
        /// <param name="classId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<List<StudentGraShowDto>> GetAllStudent(Guid teacherId,string name,string sno,Guid? classId)
        {
            List<StudentGraShowDto> list = new List<StudentGraShowDto>();
            var stus = await _studentEFRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(name),c=>c.Name == name)
                .WhereIf(!string.IsNullOrEmpty(sno),c=>c.Sno == sno)
                .WhereIf(classId != null,c=>c.ClassId == classId).ToListAsync();
            var clas = await _classesEFRepository.GetAllListAsync();
            foreach (var student in stus)
            {
                var state = false;
                var myChoose = false;
                if(student.GraDesTeacherId != null && student.GraDesTeacherId != teacherId)
                {
                    state = true;
                }
                if (student.GraDesTeacherId == teacherId)
                {
                    myChoose = true;
                }
                var cla = clas.FirstOrDefault(c => c.Id == student.ClassId);
                list.Add(new StudentGraShowDto
                {
                    StudentId = student.Id,
                    Name  = student.Name,
                    Class = cla.SchoolYear + cla.Major + cla.Name,
                    Sno = student.Sno,
                    MyChoose = myChoose,
                    State = state
                });
            }
            return list;
        }
        /// <summary>
        /// 判断课程里面有没有学生
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<bool> GetCourseHasStudents(Guid courseId)
        {
            var stus = await _studentCourseEFRepository.GetAllListAsync(c => courseId == c.CourseId);
            if(stus.Count()>0)
                return true;
            return false;
        }
        /// <summary>
        /// 已添加的学生列表
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<List<StudentGraShowDto>> GetGraDesStudent(Guid teacherId)
        {
            List<StudentGraShowDto> list = new List<StudentGraShowDto>();
            var stus = await _studentEFRepository.GetAllListAsync();
            var clas = await _classesEFRepository.GetAllListAsync();
            //老师拥有的所有学生
            var teaStus = stus.Where(c => c.GraDesTeacherId == teacherId).ToList();
            foreach (var student in teaStus)
            {
                var state = false;
                var myChoose = false;
                if (student.GraDesTeacherId != null && student.GraDesTeacherId != teacherId)
                {
                    state = true;
                }
                if (student.GraDesTeacherId == teacherId)
                {
                    myChoose = true;
                }
                var cla = clas.FirstOrDefault(c => c.Id == student.ClassId);
                list.Add(new StudentGraShowDto
                {
                    StudentId = student.Id,
                    Name = student.Name,
                    Class = cla.SchoolYear + cla.Major + cla.Name,
                    Sno = student.Sno,
                    MyChoose = myChoose,
                    State = state
                });
            }
            return list;
        }
        /// <summary>
        /// 老师选择学生毕业设计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> TeacherChooseStuGra(SelectStudent input)
        {
            var stus = await _studentEFRepository.GetAllListAsync();
            //老师拥有的所有学生
            var teaStus = stus.Where(c => c.GraDesTeacherId == input.teacherId);
            //输入的学生里面不包含之前拥有的学生
            foreach(var student in teaStus)
            {
                if (!input.stuIds.Contains(student.Id))
                {
                    student.GraDesTeacherId = null;
                    await _studentEFRepository.UpdateAsync(student);
                }
            }
            //选择的所有学生老师Id都设为当前老师
            foreach(var stuId in input.stuIds)
            {
                var stu = stus.FirstOrDefault(c => c.Id == stuId);
                stu.GraDesTeacherId = input.teacherId;
                await _studentEFRepository.UpdateAsync(stu);
            }
            return true;
        } 
        /// <summary>
        /// 获取学生课程设计下拉框
        /// </summary>
        /// <param name="stuId"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<TwoSelectDto> GetStudentDesignSelect(Guid stuId)
        {
            var classId = (await _studentEFRepository.GetAsync(stuId)).ClassId;
            var allDesign = await _courseEFRepository.GetAllListAsync(c => c.Kind == "课设");
            //该班级的所有课程
            var classCourse = await _classCourseEFRepository.GetAll().Where(c => c.ClassId == classId).Select(c => c.CourseId).ToListAsync();
            List<SelectDto<Guid>> list1 = new List<SelectDto<Guid>>();
            List<SelectDto<string>> list2 = new List<SelectDto<string>>();
            foreach (var coureId in classCourse)
            {
                var course = allDesign.FirstOrDefault(c => c.Id == coureId);
                if (course != null)
                {
                    list1.Add(new SelectDto<Guid>(course.Id, course.Name));
                    list2.Add(new SelectDto<string>(course.Semester, course.Semester));
                }
            }
            return new TwoSelectDto
            {
                List1 = list1,
                List2 = list2,
            };
        }
        /// <summary>
        /// 获取学生课程下拉框
        /// </summary>
        /// <param name="stuId"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_Roles)]
        public async Task<TwoSelectDto> GetStudentCourseelect(Guid stuId)
        {
            var classId = (await _studentEFRepository.GetAsync(stuId)).ClassId;
            var allDesign = await _courseEFRepository.GetAllListAsync(c => c.Kind == "课程");
            //该班级的所有课程
            var classCourse = await _classCourseEFRepository.GetAll().Where(c => c.ClassId == classId).Select(c => c.CourseId).ToListAsync();
            List<SelectDto<Guid>> list1 = new List<SelectDto<Guid>>();
            List<SelectDto<string>> list2 = new List<SelectDto<string>>();
            foreach (var coureId in classCourse)
            {
                var course = allDesign.FirstOrDefault(c => c.Id == coureId);
                if (course != null)
                {
                    list1.Add(new SelectDto<Guid>(course.Id, course.Name));
                    list2.Add(new SelectDto<string>(course.Semester, course.Semester));
                }
            }
            return new TwoSelectDto
            {
                List1 = list1,
                List2 = list2,
            };
        }
    }
}
