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
        public Task<IList<ApplicantMessage>> GetApplicantMessages(GetApplicantMessageOptions options, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicantMessagesGet.ApplicantId, options.ApplicantId },
                { Sprocs.ApplicantMessagesGet.CreatedOn, options.CreatedOn },
                { Sprocs.ApplicantMessagesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<ApplicantMessage>(Sprocs.ApplicantMessagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ApplicantMessage> GetApplicantMessage(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicantMessagesGet.Id, id },
                { Sprocs.ApplicantMessagesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<ApplicantMessage>(Sprocs.ApplicantMessagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
