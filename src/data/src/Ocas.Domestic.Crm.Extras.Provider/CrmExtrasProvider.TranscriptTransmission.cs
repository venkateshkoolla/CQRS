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
        public Task<IList<TranscriptTransmission>> GetTranscriptTransmissionDates(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptTransmissionsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<TranscriptTransmission>(Sprocs.TranscriptTransmissionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<TranscriptTransmission> GetTranscriptTransmissionDate(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptTransmissionsGet.Id, id },
                { Sprocs.TranscriptTransmissionsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<TranscriptTransmission>(Sprocs.TranscriptTransmissionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
