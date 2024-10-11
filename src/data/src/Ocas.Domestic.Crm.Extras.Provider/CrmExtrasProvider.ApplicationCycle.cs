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
        public Task<IList<ApplicationCycle>> GetApplicationCycles()
        {
            return Connection.QueryAsync<ApplicationCycle>(Sprocs.ApplicationCyclesGet.Sproc, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ApplicationCycle> GetApplicationCycle(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationCyclesGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<ApplicationCycle>(Sprocs.ApplicationCyclesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
