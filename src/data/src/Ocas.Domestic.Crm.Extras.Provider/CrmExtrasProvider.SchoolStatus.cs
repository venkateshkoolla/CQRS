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
        public Task<IList<SchoolStatus>> GetSchoolStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SchoolStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<SchoolStatus>(Sprocs.SchoolStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<SchoolStatus> GetSchoolStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.SchoolStatusesGet.Id, id },
                { Sprocs.SchoolStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<SchoolStatus>(Sprocs.SchoolStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
