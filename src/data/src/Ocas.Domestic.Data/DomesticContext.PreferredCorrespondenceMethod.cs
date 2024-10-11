using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<PreferredCorrespondenceMethod> GetPreferredCorrespondenceMethod(Guid preferredCorrespondenceMethodId, Locale locale)
        {
            return CrmExtrasProvider.GetPreferredCorrespondenceMethod(preferredCorrespondenceMethodId, locale);
        }

        public Task<IList<PreferredCorrespondenceMethod>> GetPreferredCorrespondenceMethods(Locale locale)
        {
            return CrmExtrasProvider.GetPreferredCorrespondenceMethods(locale);
        }
    }
}
