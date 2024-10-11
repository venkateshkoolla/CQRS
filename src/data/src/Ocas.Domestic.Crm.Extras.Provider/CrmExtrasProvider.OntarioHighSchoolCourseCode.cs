using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<OntarioHighSchoolCourseCode>> GetOntarioHighSchoolCourseCodes()
        {
            return Connection.QueryAsync<OntarioHighSchoolCourseCode>(Sprocs.OntarioHighSchoolCourseCodesGet.Sproc, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<OntarioHighSchoolCourseCode> GetOntarioHighSchoolCourseCode(string name)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OntarioHighSchoolCourseCodesGet.Name, name }
            };

            return Connection.QueryFirstOrDefaultAsync<OntarioHighSchoolCourseCode>(Sprocs.OntarioHighSchoolCourseCodesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
