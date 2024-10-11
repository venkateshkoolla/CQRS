using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Credential> GetCredential(Guid credentialId, Locale locale);
        Task<IList<Credential>> GetCredentials(Locale locale);
    }
}
