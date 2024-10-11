using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ApplicationCycleStatus> GetApplicationCycleStatus(Guid applicationCycleStatusId, Locale locale);
        Task<IList<ApplicationCycleStatus>> GetApplicationCycleStatuses(Locale locale);
    }
}
