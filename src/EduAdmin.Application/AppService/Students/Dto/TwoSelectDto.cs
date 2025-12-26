using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Students.Dto
{
    public class TwoSelectDto
    {
        public List<SelectDto<Guid>> List1 { get; set; }
        public List<SelectDto<string>> List2 { get; set; }
    }
    public class SelectStudent
    {
        public Guid teacherId { get; set; }
        public List<Guid> stuIds { get; set; }
    }
}
