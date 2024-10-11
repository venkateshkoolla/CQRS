using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<TestType> GetTestType(Guid testTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetTestType(testTypeId, locale);
        }

        public Task<IList<TestType>> GetTestTypes(Locale locale)
        {
            return CrmExtrasProvider.GetTestTypes(locale);
        }

        public Task<IList<TestType>> GetTestTypes(Locale locale, State? state = null)
        {
            return CrmExtrasProvider.GetTestTypes(locale, state);
        }
    }
}
