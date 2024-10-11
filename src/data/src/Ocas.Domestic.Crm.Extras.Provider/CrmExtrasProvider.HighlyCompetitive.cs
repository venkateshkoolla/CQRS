﻿using System;
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
        public Task<IList<HighlyCompetitive>> GetHighlyCompetitives(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighlyCompetitivesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<HighlyCompetitive>(Sprocs.HighlyCompetitivesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<HighlyCompetitive> GetHighlyCompetitive(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighlyCompetitivesGet.Id, id },
                { Sprocs.HighlyCompetitivesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<HighlyCompetitive>(Sprocs.HighlyCompetitivesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
