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
        public Task<IList<TranscriptSource>> GetTranscriptSources(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptSourcesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<TranscriptSource>(Sprocs.TranscriptSourcesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<TranscriptSource> GetTranscriptSource(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptSourcesGet.Id, id },
                { Sprocs.TranscriptSourcesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<TranscriptSource>(Sprocs.TranscriptSourcesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}