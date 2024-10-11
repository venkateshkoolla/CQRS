using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Language> GetLanguage(Guid languageId, Locale locale)
        {
            return CrmExtrasProvider.GetLanguage(languageId, locale);
        }

        public Task<IList<Language>> GetLanguages(Locale locale)
        {
            return CrmExtrasProvider.GetLanguages(locale);
        }
    }
}
