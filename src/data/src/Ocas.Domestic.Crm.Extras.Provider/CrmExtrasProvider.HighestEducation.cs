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
        public Task<IList<HighestEducation>> GetHighestEducations(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighestEducationsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<HighestEducation>(Sprocs.HighestEducationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<HighestEducation> GetHighestEducation(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighestEducationsGet.Id, id },
                { Sprocs.HighestEducationsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<HighestEducation>(Sprocs.HighestEducationsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}