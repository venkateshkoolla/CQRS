using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Offer> GetOffer(Guid offerId);
        Task<IList<Offer>> GetOffers(GetOfferOptions options);
        Task AcceptOffer(Guid offerId, string modifiedBy);
        Task DeclineOffer(Guid offerId, string modifiedBy);
    }
}
