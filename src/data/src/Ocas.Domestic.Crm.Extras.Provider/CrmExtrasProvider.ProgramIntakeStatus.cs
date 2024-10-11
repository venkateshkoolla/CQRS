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
        public Task<IList<ProgramIntakeStatus>> GetProgramIntakeStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakeStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ProgramIntakeStatus>(Sprocs.ProgramIntakeStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramIntakeStatus> GetProgramIntakeStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakeStatusesGet.Id, id },
                { Sprocs.ProgramIntakeStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramIntakeStatus>(Sprocs.ProgramIntakeStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
