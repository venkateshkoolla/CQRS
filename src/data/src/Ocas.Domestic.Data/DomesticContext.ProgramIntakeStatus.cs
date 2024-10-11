using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramIntakeStatus> GetProgramIntakeStatus(Guid programIntakeStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramIntakeStatus(programIntakeStatusId, locale);
        }

        public Task<IList<ProgramIntakeStatus>> GetProgramIntakeStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetProgramIntakeStatuses(locale);
        }
    }
}
