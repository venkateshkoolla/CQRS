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
        public Task<IList<PrivacyStatement>> GetPrivacyStatements(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PrivacyStatementsGet.Locale, (int)locale },
                { Sprocs.PrivacyStatementsGet.StateCode, State.Active },
                { Sprocs.PrivacyStatementsGet.StatusCode, PrivacyStatementStatusCode.Active }
            };

            return Connection.QueryAsync<PrivacyStatement>(Sprocs.PrivacyStatementsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<PrivacyStatement> GetLatestApplicantPrivacyStatement(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PrivacyStatementsGet.Locale, (int)locale },
                { Sprocs.PrivacyStatementsGet.StateCode, State.Active },
                { Sprocs.PrivacyStatementsGet.StatusCode, PrivacyStatementStatusCode.Active },
                { Sprocs.PrivacyStatementsGet.EffectiveDate, DateTime.UtcNow },
                { Sprocs.PrivacyStatementsGet.Category, ContactType.Applicant }
            };

            return Connection.QueryFirstOrDefaultAsync<PrivacyStatement>(Sprocs.PrivacyStatementsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<PrivacyStatement> GetPrivacyStatement(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.PrivacyStatementsGet.Id, id },
                { Sprocs.PrivacyStatementsGet.Locale, (int)locale },
                { Sprocs.PrivacyStatementsGet.StateCode, State.Active },
                { Sprocs.PrivacyStatementsGet.StatusCode, PrivacyStatementStatusCode.Active }
            };

            return Connection.QueryFirstOrDefaultAsync<PrivacyStatement>(Sprocs.PrivacyStatementsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
