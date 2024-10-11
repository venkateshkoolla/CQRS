﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CredentialEvaluationAgency> GetCredentialEvaluationAgency(Guid credentialEvaluationAgencyId);

        Task<IList<CredentialEvaluationAgency>> GetCredentialEvaluationAgencies();
    }
}
