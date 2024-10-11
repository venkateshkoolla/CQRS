using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.AppSettings.Extras;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase
    {
        public IList<Offer> MapOffers(IList<Dto.Offer> model, IList<LookupItem> offerStates, IList<LookupItem> applicationStatuses, IAppSettingsExtras appSettingsExtras)
        {
            var result = _mapper.Map<IList<Offer>>(model);

            if (result?.Any() != true) return result;

            var utcNow = DateTime.UtcNow;
            var estToday = utcNow.ToDateInEstAsUtc();

            var activeOffer = offerStates.First(x => x.Code == Constants.Offers.State.Active);
            var activeApplication = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active);

            // set lockReleaseDate on a per cycle basis
            foreach (var offersForCycle in result.GroupBy(x => x.ApplicationCycleId))
            {
                var maxLock = offersForCycle.Max(x => x.OfferLockReleaseDate);

                var offersLockedReleaseDate = maxLock.HasValue ? appSettingsExtras.GetNextSendDate(maxLock.Value) : null;

                foreach (var offer in offersForCycle)
                {
                    offer.CanRespond = false;

                    offer.OfferLockReleaseDate = offersLockedReleaseDate;
                    if (offer.OfferStateId == activeOffer.Id
                        && offer.ApplicationStatusId == activeApplication.Id
                        && estToday <= offer.HardExpiryDate.ToDateTime() // you can accept an offer up until 23:59:59 EST on the HardExpiryDate
                        && (offer.OfferLockReleaseDate == null || utcNow >= offer.OfferLockReleaseDate))
                    {
                        offer.CanRespond = true;
                    }
                }
            }

            var midnight = new TimeSpan(0, 23, 59, 59);
            foreach (var offer in result)
            {
                offer.IsSoftExpired = !string.IsNullOrEmpty(offer.SoftExpiryDate) && offer.SoftExpiryDate.ToDateTime().Add(midnight) <= utcNow;
                offer.IsHardExpired = !string.IsNullOrEmpty(offer.HardExpiryDate) && offer.HardExpiryDate.ToDateTime().Add(midnight) <= utcNow;
            }

            return result;
        }
    }
}
