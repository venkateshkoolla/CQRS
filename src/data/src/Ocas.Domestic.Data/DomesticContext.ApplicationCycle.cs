using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ApplicationCycle> GetApplicationCycle(Guid applicationCycleId)
        {
            return CrmExtrasProvider.GetApplicationCycle(applicationCycleId);
        }

        public Task<IList<ApplicationCycle>> GetApplicationCycles()
        {
            return CrmExtrasProvider.GetApplicationCycles();
        }
    }
}
