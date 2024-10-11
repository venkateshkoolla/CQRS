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
        public Task<IList<CourseDelivery>> GetCourseDeliveries(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseDeliveriesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<CourseDelivery>(Sprocs.CourseDeliveriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<CourseDelivery> GetCourseDelivery(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CourseDeliveriesGet.Id, id },
                { Sprocs.CourseDeliveriesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<CourseDelivery>(Sprocs.CourseDeliveriesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
