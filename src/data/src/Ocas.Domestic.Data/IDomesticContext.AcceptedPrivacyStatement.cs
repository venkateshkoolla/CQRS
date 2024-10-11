using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<AcceptedPrivacyStatement> AddAcceptedPrivacyStatement(Contact contact, PrivacyStatement privacyStatement, DateTime acceptedDate);
    }
}
