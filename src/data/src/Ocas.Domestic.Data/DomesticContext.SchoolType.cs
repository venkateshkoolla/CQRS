using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<SchoolType> GetSchoolType(Guid schoolTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetSchoolType(schoolTypeId, locale);
        }

        public Task<IList<SchoolType>> GetSchoolTypes(Locale locale)
        {
            return CrmExtrasProvider.GetSchoolTypes(locale);
        }
    }
}
