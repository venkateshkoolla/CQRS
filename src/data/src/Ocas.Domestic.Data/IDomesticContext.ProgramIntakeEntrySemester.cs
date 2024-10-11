using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramIntakeEntrySemester> CreateProgramIntakeEntrySemester(ProgramIntakeEntrySemesterBase model);
        Task DeleteProgramIntakeEntrySemester(Guid id);
        Task<ProgramIntakeEntrySemester> GetProgramIntakeEntrySemester(Guid id);
        Task<IList<ProgramIntakeEntrySemester>> GetProgramIntakeEntrySemesters(GetProgramIntakeEntrySemesterOptions options);
        Task<ProgramIntakeEntrySemester> UpdateProgramIntakeEntrySemester(ProgramIntakeEntrySemester model);
    }
}
