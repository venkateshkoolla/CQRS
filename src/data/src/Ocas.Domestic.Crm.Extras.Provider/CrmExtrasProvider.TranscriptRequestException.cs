using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<TranscriptRequestException> GetTranscriptRequestException(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptRequestExceptionsGet.Id, id },
                { Sprocs.TranscriptRequestExceptionsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<TranscriptRequestException>(Sprocs.TranscriptRequestExceptionsGet.Sproc, parameters, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<TranscriptRequestException>> GetTranscriptRequestExceptions(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.TranscriptRequestExceptionsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<TranscriptRequestException>(Sprocs.TranscriptRequestExceptionsGet.Sproc, parameters, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
