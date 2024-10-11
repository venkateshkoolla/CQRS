using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<PrivacyStatement> GetPrivacyStatement(Guid privacyStatementId, Locale locale)
        {
            return CrmExtrasProvider.GetPrivacyStatement(privacyStatementId, locale);
        }

        public Task<PrivacyStatement> GetLatestApplicantPrivacyStatement(Locale locale)
        {
            return CrmExtrasProvider.GetLatestApplicantPrivacyStatement(locale);
        }

        public Task<IList<PrivacyStatement>> GetPrivacyStatements(Locale locale)
        {
            return CrmExtrasProvider.GetPrivacyStatements(locale);
        }
    }
}
