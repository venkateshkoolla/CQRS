using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CollegeInformation> GetCollegeInformation(Guid collegeInformationId, Locale locale);

        Task<IList<CollegeInformation>> GetCollegeInformations(Locale locale);
    }
}
