using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<ProgramLanguage>> GetProgramLanguages(Locale locale);
        Task<ProgramLanguage> GetProgramLanguage(Guid id, Locale locale);
    }
}
