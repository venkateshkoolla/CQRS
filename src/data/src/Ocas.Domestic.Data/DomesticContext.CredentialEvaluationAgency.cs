using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CredentialEvaluationAgency> GetCredentialEvaluationAgency(Guid credentialEvaluationAgencyId)
        {
            return CrmExtrasProvider.GetCredentialEvaluationAgency(credentialEvaluationAgencyId);
        }

        public Task<IList<CredentialEvaluationAgency>> GetCredentialEvaluationAgencies()
        {
            return CrmExtrasProvider.GetCredentialEvaluationAgencies();
        }
    }
}
