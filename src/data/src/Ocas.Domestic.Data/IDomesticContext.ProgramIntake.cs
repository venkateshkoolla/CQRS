using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramIntake> ActivateProgramIntake(Guid id);
        Task<ProgramIntake> CreateProgramIntake(ProgramIntakeBase model);
        Task DeleteProgramIntake(Guid id);
        Task DeleteProgramIntake(ProgramIntake model);
        Task<ProgramIntake> GetProgramIntake(Guid id);
        Task<IList<ProgramIntake>> GetProgramIntakes(GetProgramIntakeOptions options);
        Task<IList<ProgramIntake>> GetProgramIntakes(Guid programId);
        Task<ProgramIntake> UpdateProgramIntake(ProgramIntake model);
    }
}
