using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<ShsmCompletion>> GetShsmCompletions()
        {
            return Connection.QueryAsync<ShsmCompletion>(Sprocs.ShsmCompletionsGet.Sproc, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ShsmCompletion> GetShsmCompletion(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ShsmCompletionsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<ShsmCompletion>(Sprocs.ShsmCompletionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
