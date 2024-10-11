using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<OfferState>> GetOfferStates(Locale locale);
        Task<OfferState> GetOfferState(Guid id, Locale locale);
    }
}
