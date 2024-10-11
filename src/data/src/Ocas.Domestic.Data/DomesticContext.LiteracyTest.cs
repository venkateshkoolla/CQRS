using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<LiteracyTest> GetLiteracyTest(Guid literacyTestId, Locale locale)
        {
            return CrmExtrasProvider.GetLiteracyTest(literacyTestId, locale);
        }

        public Task<IList<LiteracyTest>> GetLiteracyTests(Locale locale)
        {
            return CrmExtrasProvider.GetLiteracyTests(locale);
        }
    }
}