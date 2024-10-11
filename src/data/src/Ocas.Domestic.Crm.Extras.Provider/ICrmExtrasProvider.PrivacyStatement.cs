using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<PrivacyStatement>> GetPrivacyStatements(Locale locale);
        Task<PrivacyStatement> GetLatestApplicantPrivacyStatement(Locale locale);
        Task<PrivacyStatement> GetPrivacyStatement(Guid id, Locale locale);
    }
}
