using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<InstituteType> GetInstituteType(Guid instituteTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetInstituteType(instituteTypeId, locale);
        }

        public Task<IList<InstituteType>> GetInstituteTypes(Locale locale)
        {
            return CrmExtrasProvider.GetInstituteTypes(locale);
        }
    }
}
