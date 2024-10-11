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
        public Task<IList<CourseStatus>> GetCourseStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<CourseStatus>(Sprocs.CourseStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CourseStatus> GetCourseStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseStatusesGet.Id, id },
                { Sprocs.CourseStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<CourseStatus>(Sprocs.CourseStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
