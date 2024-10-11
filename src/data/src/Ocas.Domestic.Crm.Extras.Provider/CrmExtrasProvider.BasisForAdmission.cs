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
        public Task<IList<BasisForAdmission>> GetBasisForAdmissions(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.BasisForAdmissionsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<BasisForAdmission>(Sprocs.BasisForAdmissionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<BasisForAdmission> GetBasisForAdmission(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.BasisForAdmissionsGet.Id, id },
                { Sprocs.BasisForAdmissionsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<BasisForAdmission>(Sprocs.BasisForAdmissionsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
