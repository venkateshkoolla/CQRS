using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramType> GetProgramType(Guid programTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramType(programTypeId, locale);
        }

        public Task<IList<ProgramType>> GetProgramTypes(Locale locale)
        {
            return CrmExtrasProvider.GetProgramTypes(locale);
        }
    }
}
