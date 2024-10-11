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
        public Task<IList<OstNote>> GetOstNotes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OstNotesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<OstNote>(Sprocs.OstNotesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<OstNote> GetOstNote(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OstNotesGet.Id, id },
                { Sprocs.OstNotesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<OstNote>(Sprocs.OstNotesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}