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
        public Task<IList<TranscriptRequestStatus>> GetTranscriptRequestStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptRequestStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<TranscriptRequestStatus>(Sprocs.TranscriptRequestStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<TranscriptRequestStatus> GetTranscriptRequestStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptRequestStatusesGet.Id, id },
                { Sprocs.TranscriptRequestStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<TranscriptRequestStatus>(Sprocs.TranscriptRequestStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
