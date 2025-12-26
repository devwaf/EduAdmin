using EduAdmin.LocalTools.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.ScoreWeights
{
    public interface IScoreWeightAppService
    {
        /// <summary>
        /// 删除权重
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeleteResult> DeleteScoreWeight(Guid id);
    }
}
