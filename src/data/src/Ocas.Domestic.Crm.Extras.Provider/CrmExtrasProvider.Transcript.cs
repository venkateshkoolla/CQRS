using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<Transcript> GetTranscript(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<Transcript>(Sprocs.TranscriptsGet.Sproc, parameters, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<Transcript>> GetTranscripts(GetTranscriptOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptsGet.ContactId, options.ContactId },
                { Sprocs.TranscriptsGet.PartnerId, options.PartnerId },
                { Sprocs.TranscriptsGet.BoardMident, options.BoardMident }
            };

            return Connection.QueryAsync<Transcript>(Sprocs.TranscriptsGet.Sproc, parameters, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
