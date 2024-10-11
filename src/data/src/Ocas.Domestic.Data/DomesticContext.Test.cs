using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Test> GetTest(Guid id, Locale locale)
        {
            return CrmExtrasProvider.GetTest(id, locale);
        }

        public Task<IList<Test>> GetTests(GetTestOptions testOptions, Locale locale)
        {
            return CrmExtrasProvider.GetTests(testOptions, locale);
        }

        public async Task<Test> CreateTest(TestBase testBase, Locale locale)
        {
            if (testBase.DateTestTaken.Kind != DateTimeKind.Utc) throw new ArgumentException($"TestBase.DateTestTaken must be DateTimeKind.Utc but received: {testBase.DateTestTaken.Kind}", nameof(testBase));

            var crmEntity = CrmMapper.MapTestBase(testBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetTest(id, locale);
        }

        public async Task<Test> UpdateTest(Test test, Locale locale)
        {
            if (test.DateTestTaken.Kind != DateTimeKind.Utc) throw new ArgumentException($"Test.DateTestTaken must be DateTimeKind.Utc but received: {test.DateTestTaken.Kind}", nameof(test));

            var crmEntity = CrmProvider.Tests.Single(x => x.Id == test.Id);

            CrmMapper.PatchTest(test, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetTest(crmEntity.Id, locale);
        }

        public Task DeleteTest(Test test)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_test.EntityLogicalName, test.Id);
        }

        public Task DeleteTest(Guid id)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_test.EntityLogicalName, id);
        }
    }
}
