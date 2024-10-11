using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<SchoolType> GetSchoolType(Guid schoolTypeId, Locale locale);

        Task<IList<SchoolType>> GetSchoolTypes(Locale locale);
    }
}
