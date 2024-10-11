using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramLevel> GetProgramLevel(Guid programLevelId, Locale locale);
        Task<IList<ProgramLevel>> GetProgramLevels(Locale locale);
    }
}
