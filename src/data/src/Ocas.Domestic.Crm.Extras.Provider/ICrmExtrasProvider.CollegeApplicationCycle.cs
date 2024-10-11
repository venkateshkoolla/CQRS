using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles();
        Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles(GetCollegeApplicationsOptions options);
        Task<CollegeApplicationCycle> GetCollegeApplicationCycle(Guid id);
    }
}
