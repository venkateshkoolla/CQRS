using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<ProgramIntake> GetProgramIntake(Guid id);
        Task<IList<ProgramIntake>> GetProgramIntakes(GetProgramIntakeOptions options);
        Task<IList<ProgramIntake>> GetProgramIntakes(Guid programId);
    }
}
