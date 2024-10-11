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
        public Task<IList<InternationalCreditAssessmentStatus>> GetInternationalCreditAssessmentStatuses(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.InternationalCreditAssessmentStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<InternationalCreditAssessmentStatus>(Sprocs.InternationalCreditAssessmentStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<InternationalCreditAssessmentStatus> GetInternationalCreditAssessmentStatus(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.InternationalCreditAssessmentStatusesGet.Id, id },
                { Sprocs.InternationalCreditAssessmentStatusesGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<InternationalCreditAssessmentStatus>(Sprocs.InternationalCreditAssessmentStatusesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
