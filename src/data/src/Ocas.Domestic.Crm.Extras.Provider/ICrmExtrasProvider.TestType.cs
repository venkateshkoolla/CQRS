using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<TestType>> GetTestTypes(Locale locale);
        Task<IList<TestType>> GetTestTypes(Locale locale, State? state);
        Task<TestType> GetTestType(Guid id, Locale locale);
    }
}
