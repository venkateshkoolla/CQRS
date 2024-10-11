using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Data.Mappers;
using Ocas.Domestic.Models;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Offer> GetOffer(Guid offerId)
        {
            return CrmExtrasProvider.GetOffer(offerId);
        }

        public Task<IList<Offer>> GetOffers(GetOfferOptions options)
        {
            return CrmExtrasProvider.GetOffers(options);
        }

        public async Task AcceptOffer(Guid offerId, string modifiedBy)
        {
            var utcNow = DateTime.UtcNow;
            var offerToConfirm = CrmProvider.Offers.First(x => x.Id == offerId);
            var acceptedOfferStatusId = CrmProvider.OfferStatuses.First(os => os.ocaslr_code == Constants.Offers.Status.Accepted).Id;

            offerToConfirm.ocaslr_offerstatusid = acceptedOfferStatusId.ToEntityReference(ocaslr_offerstatus.EntityLogicalName);
            offerToConfirm.ocaslr_confirmeddate = utcNow;
            offerToConfirm.ocaslr_lock = utcNow;
            offerToConfirm.ocaslr_modifiedbyuser = modifiedBy;

            await CrmProvider.UpdateEntity(offerToConfirm);
        }

        public async Task DeclineOffer(Guid offerId, string modifiedBy)
        {
            var utcNow = DateTime.UtcNow;
            var offerToConfirm = CrmProvider.Offers.First(x => x.Id == offerId);
            var declinedOfferStatusId = CrmProvider.OfferStatuses.First(os => os.ocaslr_code == Constants.Offers.Status.Declined).Id;

            offerToConfirm.ocaslr_offerstatusid = declinedOfferStatusId.ToEntityReference(ocaslr_offerstatus.EntityLogicalName);
            offerToConfirm.ocaslr_confirmeddate = utcNow;
            offerToConfirm.ocaslr_lock = utcNow;
            offerToConfirm.ocaslr_modifiedbyuser = modifiedBy;

            await CrmProvider.UpdateEntity(offerToConfirm);
        }
    }
}
