using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Homeworks.Dto
{
    public class CompareDto : IEqualityComparer<HomeworkTeaMessageDto>
    {
        public bool Equals(HomeworkTeaMessageDto a, HomeworkTeaMessageDto b)
        {
            return a.Message == b.Message;
        }

        public int GetHashCode(HomeworkTeaMessageDto obj)
        {
            return obj.Message.GetHashCode();
        }
    }
}
