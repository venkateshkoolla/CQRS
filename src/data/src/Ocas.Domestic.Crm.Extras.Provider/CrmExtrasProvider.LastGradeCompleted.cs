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
        public Task<IList<LastGradeCompleted>> GetLastGradeCompleteds(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LastGradeCompletedsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<LastGradeCompleted>(Sprocs.LastGradeCompletedsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<LastGradeCompleted> GetLastGradeCompleted(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.LastGradeCompletedsGet.Id, id },
                { Sprocs.LastGradeCompletedsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<LastGradeCompleted>(Sprocs.LastGradeCompletedsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
