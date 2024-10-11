using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<Offer>> GetOffers(Guid applicantId)
        {
            return Get<IList<Offer>>(QueryHelpers.AddQueryString(Constants.Route.Offers, "applicantId", applicantId.ToString()));
        }

        public Task<IList<Offer>> AcceptOffer(Guid offerId)
        {
            return Post<IList<Offer>>($"{Constants.Route.Offers}/{offerId}/accept");
        }

        public Task<IList<Offer>> DeclineAllOffers(DeclineAllOffersInfo declineAllOffersInfo)
        {
            return Post<IList<Offer>>($"{Constants.Route.Offers}/decline-all", declineAllOffersInfo);
        }

        public Task<IList<Offer>> DeclineOffer(Guid offerId)
        {
            return Post<IList<Offer>>($"{Constants.Route.Offers}/{offerId}/decline");
        }
    }
}
