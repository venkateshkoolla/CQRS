using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramLevel> GetProgramLevel(Guid programLevelId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramLevel(programLevelId, locale);
        }

        public Task<IList<ProgramLevel>> GetProgramLevels(Locale locale)
        {
            return CrmExtrasProvider.GetProgramLevels(locale);
        }
    }
}
