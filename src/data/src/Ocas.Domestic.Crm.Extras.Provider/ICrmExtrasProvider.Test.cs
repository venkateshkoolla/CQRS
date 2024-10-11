using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<Test> GetTest(Guid id, Locale locale);
        Task<IList<Test>> GetTests(GetTestOptions testOptions, Locale locale);
    }
}
