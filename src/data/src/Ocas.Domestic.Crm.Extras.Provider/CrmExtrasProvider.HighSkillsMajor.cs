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
        public Task<IList<HighSkillsMajor>> GetHighSkillsMajors(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighSkillsMajorsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<HighSkillsMajor>(Sprocs.HighSkillsMajorsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<HighSkillsMajor> GetHighSkillsMajor(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighSkillsMajorsGet.Id, id },
                { Sprocs.HighSkillsMajorsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<HighSkillsMajor>(Sprocs.HighSkillsMajorsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}