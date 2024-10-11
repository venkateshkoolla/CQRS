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
        public Task<IList<AdultTraining>> GetAdultTrainings(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AdultTrainingsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<AdultTraining>(Sprocs.AdultTrainingsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<AdultTraining> GetAdultTraining(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AdultTrainingsGet.Id, id },
                { Sprocs.AdultTrainingsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<AdultTraining>(Sprocs.AdultTrainingsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
