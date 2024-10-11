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
        public Task<IList<LevelOfStudy>> GetLevelOfStudies(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LevelOfStudiesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<LevelOfStudy>(Sprocs.LevelOfStudiesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<LevelOfStudy> GetLevelOfStudy(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LevelOfStudiesGet.Id, id },
                { Sprocs.LevelOfStudiesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<LevelOfStudy>(Sprocs.LevelOfStudiesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
