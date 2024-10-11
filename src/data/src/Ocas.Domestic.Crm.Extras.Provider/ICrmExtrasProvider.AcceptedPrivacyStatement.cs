using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<AcceptedPrivacyStatement> GetAcceptedPrivacyStatement(Guid id);
    }
}
