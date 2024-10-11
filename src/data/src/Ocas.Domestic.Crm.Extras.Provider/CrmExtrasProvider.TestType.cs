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
        public Task<IList<TestType>> GetTestTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TestTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<TestType>(Sprocs.TestTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<IList<TestType>> GetTestTypes(Locale locale, State? state = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TestTypesGet.Locale, (int)locale },
                { Sprocs.TestTypesGet.StateCode, (int?)state }
            };

            return Connection.QueryAsync<TestType>(Sprocs.TestTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<TestType> GetTestType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TestTypesGet.Id, id },
                { Sprocs.TestTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<TestType>(Sprocs.TestTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
