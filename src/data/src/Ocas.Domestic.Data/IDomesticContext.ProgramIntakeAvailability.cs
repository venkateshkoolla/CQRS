using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramIntakeAvailability> GetProgramIntakeAvailability(Guid programIntakeAvailabilityId, Locale locale);

        Task<IList<ProgramIntakeAvailability>> GetProgramIntakeAvailabilities(Locale locale);
    }
}