using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetLatestPrivacyStatement : IRequest<PrivacyStatement>, IIdentityUser
    {
        public IPrincipal User { get; set; }
    }
}
