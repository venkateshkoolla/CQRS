using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<OfferStudyMethod> GetOfferStudyMethod(Guid offerStudyMethodId, Locale locale)
        {
            return CrmExtrasProvider.GetOfferStudyMethod(offerStudyMethodId, locale);
        }

        public Task<IList<OfferStudyMethod>> GetOfferStudyMethods(Locale locale)
        {
            return CrmExtrasProvider.GetOfferStudyMethods(locale);
        }
    }
}
