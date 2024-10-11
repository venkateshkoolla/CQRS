using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<HighSchool> GetHighSchool(Guid highSchoolId, Locale locale)
        {
            return CrmExtrasProvider.GetHighSchool(highSchoolId, locale);
        }

        public Task<IList<HighSchool>> GetHighSchools(Locale locale)
        {
            return CrmExtrasProvider.GetHighSchools(locale);
        }
    }
}
