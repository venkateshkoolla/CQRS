using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<SchoolStatus> GetSchoolStatus(Guid schoolStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetSchoolStatus(schoolStatusId, locale);
        }

        public Task<IList<SchoolStatus>> GetSchoolStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetSchoolStatuses(locale);
        }
    }
}
