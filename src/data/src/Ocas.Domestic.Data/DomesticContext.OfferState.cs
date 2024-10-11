using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<OfferState> GetOfferState(Guid offerStateId, Locale locale)
        {
            return CrmExtrasProvider.GetOfferState(offerStateId, locale);
        }

        public Task<IList<OfferState>> GetOfferStates(Locale locale)
        {
            return CrmExtrasProvider.GetOfferStates(locale);
        }
    }
}
