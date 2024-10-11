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
        public Task<IList<ApplicationStatus>> GetApplicationStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ApplicationStatus>(Sprocs.ApplicationStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ApplicationStatus> GetApplicationStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationStatusesGet.Id, id },
                { Sprocs.ApplicationStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ApplicationStatus>(Sprocs.ApplicationStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
