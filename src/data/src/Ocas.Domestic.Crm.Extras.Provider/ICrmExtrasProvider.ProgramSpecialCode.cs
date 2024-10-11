using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes();
        Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes(Guid collegeApplicationId);
        Task<ProgramSpecialCode> GetProgramSpecialCode(Guid id);
    }
}
