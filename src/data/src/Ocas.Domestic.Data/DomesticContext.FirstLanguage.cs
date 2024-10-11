using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<FirstLanguage> GetFirstLanguage(Guid firstLanguageId, Locale locale)
        {
            return CrmExtrasProvider.GetFirstLanguage(firstLanguageId, locale);
        }

        public Task<IList<FirstLanguage>> GetFirstLanguages(Locale locale)
        {
            return CrmExtrasProvider.GetFirstLanguages(locale);
        }
    }
}
