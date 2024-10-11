using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OfferType> GetOfferType(Guid offerTypeId, Locale locale);
        Task<IList<OfferType>> GetOfferTypes(Locale locale);
    }
}
