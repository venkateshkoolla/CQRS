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
        public Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles()
        {
            return Connection.QueryAsync<CollegeApplicationCycle>(Sprocs.CollegeApplicationCyclesGet.Sproc, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<IList<CollegeApplicationCycle>> GetCollegeApplicationCycles(GetCollegeApplicationsOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CollegeApplicationCyclesGet.CollegeId, options.CollegeId },
                { Sprocs.CollegeApplicationCyclesGet.ApplicationCycleId, options.ApplicationCycleId },
                { Sprocs.CollegeApplicationCyclesGet.StateCode, options.StateCode }
            };

            return Connection.QueryAsync<CollegeApplicationCycle>(Sprocs.CollegeApplicationCyclesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CollegeApplicationCycle> GetCollegeApplicationCycle(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CollegeApplicationCyclesGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<CollegeApplicationCycle>(Sprocs.CollegeApplicationCyclesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
