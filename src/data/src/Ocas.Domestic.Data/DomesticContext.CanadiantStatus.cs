using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CanadianStatus> GetCanadianStatus(Guid canadianStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetCanadianStatus(canadianStatusId, locale);
        }

        public Task<IList<CanadianStatus>> GetCanadianStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetCanadianStatuses(locale);
        }
    }
}
