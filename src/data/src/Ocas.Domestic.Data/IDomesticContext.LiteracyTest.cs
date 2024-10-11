using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<LiteracyTest> GetLiteracyTest(Guid literacyTestId, Locale locale);
        Task<IList<LiteracyTest>> GetLiteracyTests(Locale locale);
    }
}