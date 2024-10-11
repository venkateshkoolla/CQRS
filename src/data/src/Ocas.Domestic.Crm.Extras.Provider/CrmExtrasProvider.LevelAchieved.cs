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
        public Task<IList<LevelAchieved>> GetLevelAchieveds(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LevelAchievedsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<LevelAchieved>(Sprocs.LevelAchievedsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<LevelAchieved> GetLevelAchieved(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LevelAchievedsGet.Id, id },
                { Sprocs.LevelAchievedsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<LevelAchieved>(Sprocs.LevelAchievedsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
