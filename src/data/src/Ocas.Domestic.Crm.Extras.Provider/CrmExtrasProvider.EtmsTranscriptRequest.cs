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
        public Task<EtmsTranscriptRequest> GetEtmsTranscriptRequest(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.EtmsTranscriptRequestsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<EtmsTranscriptRequest>(Sprocs.EtmsTranscriptRequestsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
