using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Students.Dto
{
    public class StudentGraShowDto
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Sno { get; set; }
        public bool MyChoose { get; set; }
        public bool State { get; set; }
    }
}
