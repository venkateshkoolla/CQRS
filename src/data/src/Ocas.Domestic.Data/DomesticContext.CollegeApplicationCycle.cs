using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles()
        {
            return CrmExtrasProvider.GetCollegeApplicationCycles();
        }

        public Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles(GetCollegeApplicationsOptions options)
        {
            return CrmExtrasProvider.GetCollegeApplicationCycles(options);
        }

        public Task<CollegeApplicationCycle> GetCollegeApplicationCycle(Guid collegeApplicationCycleId)
        {
            return CrmExtrasProvider.GetCollegeApplicationCycle(collegeApplicationCycleId);
        }
    }
}
