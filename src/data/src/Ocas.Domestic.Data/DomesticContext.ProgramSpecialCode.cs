using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramSpecialCode> GetProgramSpecialCode(Guid programSpecialCodeId)
        {
            return CrmExtrasProvider.GetProgramSpecialCode(programSpecialCodeId);
        }

        public Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes()
        {
            return CrmExtrasProvider.GetProgramSpecialCodes();
        }

        public Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes(Guid collegeApplicationId)
        {
            return CrmExtrasProvider.GetProgramSpecialCodes(collegeApplicationId);
        }
    }
}
