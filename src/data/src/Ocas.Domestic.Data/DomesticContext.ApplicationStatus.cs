using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ApplicationStatus> GetApplicationStatus(Guid applicationStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetApplicationStatus(applicationStatusId, locale);
        }

        public Task<IList<ApplicationStatus>> GetApplicationStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetApplicationStatuses(locale);
        }
    }
}
