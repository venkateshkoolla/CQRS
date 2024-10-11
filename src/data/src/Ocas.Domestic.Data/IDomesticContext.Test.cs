using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Test> GetTest(Guid id, Locale locale);
        Task<IList<Test>> GetTests(GetTestOptions testOptions, Locale locale);
        Task<Test> CreateTest(TestBase testBase, Locale locale);
        Task<Test> UpdateTest(Test test, Locale locale);
        Task DeleteTest(Test test);
        Task DeleteTest(Guid id);
    }
}
