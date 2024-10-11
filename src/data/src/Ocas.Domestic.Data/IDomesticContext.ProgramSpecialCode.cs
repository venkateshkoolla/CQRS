using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramSpecialCode> GetProgramSpecialCode(Guid programSpecialCodeId);
        Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes();
        Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes(Guid collegeApplicationId);
    }
}
