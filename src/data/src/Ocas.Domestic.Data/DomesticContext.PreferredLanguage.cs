using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<PreferredLanguage> GetPreferredLanguage(Guid preferredLanguageId, Locale locale)
        {
            return CrmExtrasProvider.GetPreferredLanguage(preferredLanguageId, locale);
        }

        public Task<IList<PreferredLanguage>> GetPreferredLanguages(Locale locale)
        {
            return CrmExtrasProvider.GetPreferredLanguages(locale);
        }
    }
}
