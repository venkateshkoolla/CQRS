using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CollegeInformation> GetCollegeInformation(Guid collegeInformationId, Locale locale)
        {
            return CrmExtrasProvider.GetCollegeInformation(collegeInformationId, locale);
        }

        public Task<IList<CollegeInformation>> GetCollegeInformations(Locale locale)
        {
            return CrmExtrasProvider.GetCollegeInformations(locale);
        }
    }
}
