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
        public Task<ProgramEntryLevel> GetProgramEntryLevel(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramEntryLevelsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramEntryLevel>(Sprocs.ProgramEntryLevelsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<ProgramEntryLevel>> GetProgramEntryLevels(GetProgramEntryLevelOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramEntryLevelsGet.StateCode, (int)options.StateCode },
                { Sprocs.ProgramEntryLevelsGet.StatusCode, (int)options.StatusCode },
                { Sprocs.ProgramEntryLevelsGet.ProgramId, options.ProgramId },
                { Sprocs.ProgramEntryLevelsGet.EntryLevelId, options.EntryLevelId }
            };

            return Connection.QueryAsync<ProgramEntryLevel>(Sprocs.ProgramEntryLevelsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
