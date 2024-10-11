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
        public Task<Application> GetApplication(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<Application>(Sprocs.ApplicationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<Application>> GetApplications(Guid applicantId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationsGet.ApplicantId, applicantId }
            };

            return Connection.QueryAsync<Application>(Sprocs.ApplicationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<IList<Application>> GetApplications(string number)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ApplicationsGet.ApplicationNumber, number },
                { Sprocs.ApplicationsGet.ApplicationCycleStatus, null }
            };

            return Connection.QueryAsync<Application>(Sprocs.ApplicationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
