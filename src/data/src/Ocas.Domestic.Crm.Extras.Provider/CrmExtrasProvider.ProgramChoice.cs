using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Crm.Extras.Provider.Sprocs;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<ProgramChoice>> GetProgramChoices(GetProgramChoicesOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { ProgramChoicesGet.StateCode, (int)options.StateCode },
                { ProgramChoicesGet.ApplicationId, options.ApplicationId },
                { ProgramChoicesGet.ApplicantId, options.ApplicantId }
            };

            return Connection.QueryAsync<ProgramChoice>(ProgramChoicesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramChoice> GetProgramChoice(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { ProgramChoicesGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramChoice>(ProgramChoicesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<bool> HasProgramChoices(Guid applicationId)
        {
            var parameters = new Dictionary<string, object>
            {
                { ProgramChoicesAny.ApplicationId, applicationId }
            };

            return Connection.ExecuteScalarAsync<bool>(ProgramChoicesAny.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
