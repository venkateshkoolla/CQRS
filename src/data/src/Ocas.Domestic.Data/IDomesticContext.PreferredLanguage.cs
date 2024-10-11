using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<PreferredLanguage> GetPreferredLanguage(Guid preferredLanguageId, Locale locale);
        Task<IList<PreferredLanguage>> GetPreferredLanguages(Locale locale);
    }
}
