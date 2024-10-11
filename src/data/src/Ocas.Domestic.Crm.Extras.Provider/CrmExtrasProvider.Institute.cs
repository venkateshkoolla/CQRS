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
        public Task<Institute> GetInstitute(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.InstitutesGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<Institute>(Sprocs.InstitutesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<Institute>> GetInstitutes()
        {
            return Connection.QueryAsync<Institute>(Sprocs.InstitutesGet.Sproc, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
