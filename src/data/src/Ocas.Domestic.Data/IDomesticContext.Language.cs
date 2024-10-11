using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Language> GetLanguage(Guid languageId, Locale locale);
        Task<IList<Language>> GetLanguages(Locale locale);
    }
}
