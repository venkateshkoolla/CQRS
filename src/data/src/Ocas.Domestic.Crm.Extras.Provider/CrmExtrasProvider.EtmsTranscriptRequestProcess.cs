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
        public Task<EtmsTranscriptRequestProcess> GetEtmsTranscriptRequestProcess(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.EtmsTranscriptRequestProcessesGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<EtmsTranscriptRequestProcess>(Sprocs.EtmsTranscriptRequestProcessesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<EtmsTranscriptRequestProcess>> GetEtmsTranscriptRequestProcesses(Guid etmsTranscriptRequestId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.EtmsTranscriptRequestProcessesGet.EtmsTranscriptRequestId, etmsTranscriptRequestId }
            };

            return Connection.QueryAsync<EtmsTranscriptRequestProcess>(Sprocs.EtmsTranscriptRequestProcessesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
