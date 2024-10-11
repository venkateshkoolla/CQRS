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
        public Task<IList<PreferredSponsorAgency>> GetPreferredSponsorAgencies(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PreferredSponsorAgenciesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<PreferredSponsorAgency>(Sprocs.PreferredSponsorAgenciesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<PreferredSponsorAgency> GetPreferredSponsorAgency(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PreferredSponsorAgenciesGet.Id, id },
                { Sprocs.PreferredSponsorAgenciesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<PreferredSponsorAgency>(Sprocs.PreferredSponsorAgenciesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
