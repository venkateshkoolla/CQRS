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
        public Task<TranscriptRequestLog> GetTranscriptRequestLog(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptRequestLogsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<TranscriptRequestLog>(Sprocs.TranscriptRequestLogsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
