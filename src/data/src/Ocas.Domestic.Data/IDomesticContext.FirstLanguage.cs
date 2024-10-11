using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<FirstLanguage> GetFirstLanguage(Guid firstLanguageId, Locale locale);
        Task<IList<FirstLanguage>> GetFirstLanguages(Locale locale);
    }
}
