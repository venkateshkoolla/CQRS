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
        public Task<IList<CourseLanguage>> GetCourseLanguages(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<CourseLanguage>(Sprocs.CourseLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CourseLanguage> GetCourseLanguage(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseLanguagesGet.Id, id },
                { Sprocs.CourseLanguagesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<CourseLanguage>(Sprocs.CourseLanguagesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
