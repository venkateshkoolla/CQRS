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
        public Task<OntarioStudentCourseCredit> GetOntarioStudentCourseCredit(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OntarioStudentCourseCreditsGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<OntarioStudentCourseCredit>(Sprocs.OntarioStudentCourseCreditsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<OntarioStudentCourseCredit>> GetOntarioStudentCourseCredits(GetOntarioStudentCourseCreditOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OntarioStudentCourseCreditsGet.ApplicantId, options.ApplicantId },
                { Sprocs.OntarioStudentCourseCreditsGet.TranscriptId, options.TranscriptId },
                { Sprocs.OntarioStudentCourseCreditsGet.StateCode, options.State }
            };

            return Connection.QueryAsync<OntarioStudentCourseCredit>(Sprocs.OntarioStudentCourseCreditsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
