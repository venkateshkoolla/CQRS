using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<TestType> GetTestType(Guid testTypeId, Locale locale);

        Task<IList<TestType>> GetTestTypes(Locale locale);
        Task<IList<TestType>> GetTestTypes(Locale locale, State? state);
    }
}
