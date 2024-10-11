using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramEntryLevel> CreateProgramEntryLevel(ProgramEntryLevelBase model);
        Task DeleteProgramEntryLevel(Guid id);
        Task<ProgramEntryLevel> GetProgramEntryLevel(Guid id);
        Task<IList<ProgramEntryLevel>> GetProgramEntryLevels(GetProgramEntryLevelOptions options);
        Task<ProgramEntryLevel> UpdateProgramEntryLevel(ProgramEntryLevel model);
    }
}
