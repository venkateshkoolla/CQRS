using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OfferStatus> GetOfferStatus(Guid offerStatusId, Locale locale);
        Task<IList<OfferStatus>> GetOfferStatuses(Locale locale);
    }
}
