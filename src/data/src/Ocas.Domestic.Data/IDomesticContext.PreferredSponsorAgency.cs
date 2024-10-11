using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<PreferredSponsorAgency> GetPreferredSponsorAgency(Guid preferredSponsorAgencyId, Locale locale);
        Task<IList<PreferredSponsorAgency>> GetPreferredSponsorAgencies(Locale locale);
    }
}
