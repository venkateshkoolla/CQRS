using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<OfferType> GetOfferType(Guid offerTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetOfferType(offerTypeId, locale);
        }

        public Task<IList<OfferType>> GetOfferTypes(Locale locale)
        {
            return CrmExtrasProvider.GetOfferTypes(locale);
        }
    }
}
