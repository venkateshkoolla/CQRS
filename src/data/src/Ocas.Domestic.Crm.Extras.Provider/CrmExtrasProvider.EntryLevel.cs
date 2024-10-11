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
        public Task<IList<EntryLevel>> GetEntryLevels(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.EntryLevelsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<EntryLevel>(Sprocs.EntryLevelsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<EntryLevel> GetEntryLevel(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.EntryLevelsGet.Id, id },
                { Sprocs.EntryLevelsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<EntryLevel>(Sprocs.EntryLevelsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
