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
        public Task<Education> GetEducation(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.EducationsGet.StateCode, State.Active },
                { Sprocs.EducationsGet.StatusCode, Status.Active },
                { Sprocs.EducationsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<Education>(Sprocs.EducationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<Education>> GetEducations(Guid applicantId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.EducationsGet.StateCode, State.Active },
                { Sprocs.EducationsGet.StatusCode, Status.Active },
                { Sprocs.EducationsGet.ApplicantId, applicantId }
            };

            return Connection.QueryAsync<Education>(Sprocs.EducationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
