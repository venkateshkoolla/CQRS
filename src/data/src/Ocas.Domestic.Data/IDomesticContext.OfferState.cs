using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OfferState> GetOfferState(Guid offerStateId, Locale locale);
        Task<IList<OfferState>> GetOfferStates(Locale locale);
    }
}
