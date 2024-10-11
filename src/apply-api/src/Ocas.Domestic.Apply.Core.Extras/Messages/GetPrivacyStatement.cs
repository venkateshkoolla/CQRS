using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetPrivacyStatement : IRequest<PrivacyStatement>, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public Guid Id { get; set; }
    }
}
