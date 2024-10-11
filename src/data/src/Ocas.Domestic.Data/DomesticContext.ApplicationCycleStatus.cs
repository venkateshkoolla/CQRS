using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ApplicationCycleStatus> GetApplicationCycleStatus(Guid applicationCycleStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetApplicationCycleStatus(applicationCycleStatusId, locale);
        }

        public Task<IList<ApplicationCycleStatus>> GetApplicationCycleStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetApplicationCycleStatuses(locale);
        }
    }
}
