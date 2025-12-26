using EduAdmin.AppService.Outlines.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.Outlines
{
    public interface IOutlineAppService
    {
        Task<List<OutlineShowDto>> GetAllCourseOutline(Guid outlineId);
    }
}
