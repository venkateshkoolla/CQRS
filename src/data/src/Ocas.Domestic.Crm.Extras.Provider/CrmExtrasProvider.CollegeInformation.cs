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
        public Task<IList<CollegeInformation>> GetCollegeInformations(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CollegeInformationsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<CollegeInformation>(Sprocs.CollegeInformationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CollegeInformation> GetCollegeInformation(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CollegeInformationsGet.Id, id },
                { Sprocs.CollegeInformationsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<CollegeInformation>(Sprocs.CollegeInformationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
