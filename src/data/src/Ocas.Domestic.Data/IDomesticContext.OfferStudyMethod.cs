using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<OfferStudyMethod> GetOfferStudyMethod(Guid offerStudyMethodId, Locale locale);
        Task<IList<OfferStudyMethod>> GetOfferStudyMethods(Locale locale);
    }
}
