using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public async Task<Test> GetTest(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TestsGet.Id, id },
                { Sprocs.TestTypesGet.Locale, (int)locale }
            };

            // https://dapper-tutorial.net/result-multi-mapping#example-query-multi-mapping-one-to-many
            var testDictionary = new Dictionary<Guid, Test>();

            await Connection.QueryAsync<Test, TestDetail, Test>(
                Sprocs.TestsGet.Sproc,
                (test, detail) =>
                {
                    if (!testDictionary.TryGetValue(test.Id, out var testEntry))
                    {
                        testEntry = test;
                        testEntry.Details = new List<TestDetail>();
                        testDictionary.Add(testEntry.Id, testEntry);
                    }

                    if (detail != null)
                    {
                        testEntry.Details.Add(detail);
                    }

                    return testEntry;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout);

            return testDictionary.Values.FirstOrDefault();
        }

        public async Task<IList<Test>> GetTests(GetTestOptions testOptions, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TestsGet.ApplicantId, testOptions.ApplicantId },
                { Sprocs.TestTypesGet.Locale, (int)locale },
                { Sprocs.TestsGet.IsOfficial, testOptions.IsOfficial }
            };

            // https://dapper-tutorial.net/result-multi-mapping#example-query-multi-mapping-one-to-many
            var testDictionary = new Dictionary<Guid, Test>();

            await Connection.QueryAsync<Test, TestDetail, Test>(
                Sprocs.TestsGet.Sproc,
                (test, detail) =>
                {
                    if (!testDictionary.TryGetValue(test.Id, out var testEntry))
                    {
                        testEntry = test;
                        testEntry.Details = new List<TestDetail>();
                        testDictionary.Add(testEntry.Id, testEntry);
                    }

                    if (detail != null)
                    {
                        testEntry.Details.Add(detail);
                    }

                    return testEntry;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout);

            return testDictionary.Values.ToList();
        }
    }
}
