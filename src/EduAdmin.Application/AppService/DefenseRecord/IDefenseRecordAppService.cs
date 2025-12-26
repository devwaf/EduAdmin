using EduAdmin.AppService.Courses.Dto;
using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService
{
    public interface IDefenseRecordAppService
    {
        Task<ResultDto> AddDesignDefenseObjective(CreateDefenseRecordDto input);
        Task<UpdateResult> UpdateDesignDefenseObjective(UpdateDefenseRecordDto input);
        Task<DeleteResult> DeleteDesignDefenseObjective(Guid id);
        Task<DefenseRecordShowDto> GetDesignDefense(Guid studentId, Guid courseDesignId);

    }
}
