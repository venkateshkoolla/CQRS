using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ApplicationStatus> GetApplicationStatus(Guid applicationStatusId, Locale locale);

        Task<IList<ApplicationStatus>> GetApplicationStatuses(Locale locale);
    }
}
