using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramIntakeAvailability> GetProgramIntakeAvailability(Guid programIntakeAvailabilityId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramIntakeAvailability(programIntakeAvailabilityId, locale);
        }

        public Task<IList<ProgramIntakeAvailability>> GetProgramIntakeAvailabilities(Locale locale)
        {
            return CrmExtrasProvider.GetProgramIntakeAvailabilities(locale);
        }
    }
}
