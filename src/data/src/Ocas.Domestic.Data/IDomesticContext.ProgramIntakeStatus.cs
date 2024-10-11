using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramIntakeStatus> GetProgramIntakeStatus(Guid programIntakeStatusId, Locale locale);
        Task<IList<ProgramIntakeStatus>> GetProgramIntakeStatuses(Locale locale);
    }
}
