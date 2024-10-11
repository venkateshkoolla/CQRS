using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Program> ActivateProgram(Guid programId);
        Task<Program> CreateProgram(ProgramBase model);
        Task DeleteProgram(Program model);
        Task<Program> GetProgram(Guid programId);
        Task<IList<ProgramApplication>> GetProgramApplications(GetProgramApplicationsOptions options);
        Task<IList<Program>> GetPrograms(GetProgramsOptions options);
        Task<Program> UpdateProgram(Program model);
    }
}