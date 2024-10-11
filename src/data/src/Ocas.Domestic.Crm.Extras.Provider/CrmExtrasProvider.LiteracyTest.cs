using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<LiteracyTest>> GetLiteracyTests(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LiteracyTestsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<LiteracyTest>(Sprocs.LiteracyTestsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<LiteracyTest> GetLiteracyTest(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LiteracyTestsGet.Id, id },
                { Sprocs.LiteracyTestsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<LiteracyTest>(Sprocs.LiteracyTestsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}