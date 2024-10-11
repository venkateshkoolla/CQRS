using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<PrivacyStatement> GetPrivacyStatement(Guid privacyStatementId, Locale locale);
        Task<PrivacyStatement> GetLatestApplicantPrivacyStatement(Locale locale);
        Task<IList<PrivacyStatement>> GetPrivacyStatements(Locale locale);
    }
}
