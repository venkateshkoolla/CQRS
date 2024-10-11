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
        public Task<IList<ProgramIntakeAvailability>> GetProgramIntakeAvailabilities(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakeAvailabilitiesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ProgramIntakeAvailability>(Sprocs.ProgramIntakeAvailabilitiesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramIntakeAvailability> GetProgramIntakeAvailability(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakeAvailabilitiesGet.Id, id },
                { Sprocs.ProgramIntakeAvailabilitiesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramIntakeAvailability>(Sprocs.ProgramIntakeAvailabilitiesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
