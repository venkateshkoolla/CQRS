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
        public Task<InternationalCreditAssessment> GetInternationalCreditAssessment(Guid internationalCreditAssessmentId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.InternationalCreditAssessmentsGet.Id, internationalCreditAssessmentId }
            };

            return Connection.QueryFirstOrDefaultAsync<InternationalCreditAssessment>(Sprocs.InternationalCreditAssessmentsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<InternationalCreditAssessment>> GetInternationalCreditAssessments(Guid applicantId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.InternationalCreditAssessmentsGet.ApplicantId, applicantId }
            };

            return Connection.QueryAsync<InternationalCreditAssessment>(Sprocs.InternationalCreditAssessmentsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
