using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<OfferStatus> GetOfferStatus(Guid offerStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetOfferStatus(offerStatusId, locale);
        }

        public Task<IList<OfferStatus>> GetOfferStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetOfferStatuses(locale);
        }
    }
}
