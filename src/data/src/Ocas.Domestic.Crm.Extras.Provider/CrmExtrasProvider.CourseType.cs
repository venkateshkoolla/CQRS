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
        public Task<IList<CourseType>> GetCourseTypes(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseTypesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<CourseType>(Sprocs.CourseTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CourseType> GetCourseType(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseTypesGet.Id, id },
                { Sprocs.CourseTypesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<CourseType>(Sprocs.CourseTypesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
