using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<Program> GetProgram(Guid id);
        Task<IList<ProgramApplication>> GetProgramApplications(GetProgramApplicationsOptions options);
        Task<IList<Program>> GetPrograms(GetProgramsOptions programOptions);
    }
}
