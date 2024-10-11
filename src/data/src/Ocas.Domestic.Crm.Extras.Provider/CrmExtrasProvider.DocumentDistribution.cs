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
        public Task<IList<DocumentDistribution>> GetDocumentDistributions(Guid applicationId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.DocumentDistributionGet.ApplicationId, applicationId }
            };

            return Connection.QueryAsync<DocumentDistribution>(Sprocs.DocumentDistributionGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
