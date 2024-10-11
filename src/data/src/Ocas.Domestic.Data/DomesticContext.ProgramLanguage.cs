using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramLanguage> GetProgramLanguage(Guid programLanguageId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramLanguage(programLanguageId, locale);
        }

        public Task<IList<ProgramLanguage>> GetProgramLanguages(Locale locale)
        {
            return CrmExtrasProvider.GetProgramLanguages(locale);
        }
    }
}
