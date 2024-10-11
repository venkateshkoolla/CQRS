using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<IList<OfferAcceptance>> GetOfferAcceptances(GetOfferAcceptancesOptions offerAcceptancesOptions, Locale locale)
        {
            return CrmExtrasProvider.GetOfferAcceptances(offerAcceptancesOptions, locale);
        }

        public Task<OfferAcceptance> GetOfferAcceptance(Guid offerAcceptanceId, Locale locale)
        {
            return CrmExtrasProvider.GetOfferAcceptance(offerAcceptanceId, locale);
        }
    }
}