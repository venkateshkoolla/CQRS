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
        public Task<TranscriptRequest> GetTranscriptRequest(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptRequestsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<TranscriptRequest>(Sprocs.TranscriptRequestsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<TranscriptRequest>> GetTranscriptRequests(GetTranscriptRequestOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptRequestsGet.ApplicantId, options.ApplicantId },
                { Sprocs.TranscriptRequestsGet.ApplicationId, options.ApplicationId },
                { Sprocs.TranscriptRequestsGet.EducationId, options.EducationId },
                { Sprocs.TranscriptRequestsGet.StateCode, options.State },
                { Sprocs.TranscriptRequestsGet.StatusCode, options.Status }
            };

            return Connection.QueryAsync<TranscriptRequest>(Sprocs.TranscriptRequestsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
