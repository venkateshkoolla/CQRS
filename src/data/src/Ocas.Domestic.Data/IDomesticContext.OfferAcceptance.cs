using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<IList<OfferAcceptance>> GetOfferAcceptances(GetOfferAcceptancesOptions offerAcceptancesOptions, Locale locale);
        Task<OfferAcceptance> GetOfferAcceptance(Guid offerAcceptanceId, Locale locale);
    }
}