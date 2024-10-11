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
        public Task<IList<ApplicationCycleStatus>> GetApplicationCycleStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationCycleStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ApplicationCycleStatus>(Sprocs.ApplicationCycleStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ApplicationCycleStatus> GetApplicationCycleStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationCycleStatusesGet.Id, id },
                { Sprocs.ApplicationCycleStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ApplicationCycleStatus>(Sprocs.ApplicationCycleStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
