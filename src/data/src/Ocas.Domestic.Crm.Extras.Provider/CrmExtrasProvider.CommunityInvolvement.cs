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
        public Task<IList<CommunityInvolvement>> GetCommunityInvolvements(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CommunityInvolvementsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<CommunityInvolvement>(Sprocs.CommunityInvolvementsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CommunityInvolvement> GetCommunityInvolvement(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CommunityInvolvementsGet.Id, id },
                { Sprocs.CommunityInvolvementsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<CommunityInvolvement>(Sprocs.CommunityInvolvementsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}