using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<CustomAudit>> GetCustomAudits(GetCustomAuditOptions options, Locale locale)
        {
            return CrmExtrasProvider.GetCustomAudits(options, locale);
        }

        public Task<CustomAudit> GetCustomAudit(Guid id, Locale locale)
        {
            return CrmExtrasProvider.GetCustomAudit(id, locale);
        }
    }
}
