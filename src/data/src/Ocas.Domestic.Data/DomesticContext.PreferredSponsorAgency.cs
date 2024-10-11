using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<PreferredSponsorAgency> GetPreferredSponsorAgency(Guid preferredSponsorAgencyId, Locale locale)
        {
            return CrmExtrasProvider.GetPreferredSponsorAgency(preferredSponsorAgencyId, locale);
        }

        public Task<IList<PreferredSponsorAgency>> GetPreferredSponsorAgencies(Locale locale)
        {
            return CrmExtrasProvider.GetPreferredSponsorAgencies(locale);
        }
    }
}
