using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<ProgramEntryLevel> GetProgramEntryLevel(Guid id);
        Task<IList<ProgramEntryLevel>> GetProgramEntryLevels(GetProgramEntryLevelOptions options);
    }
}
