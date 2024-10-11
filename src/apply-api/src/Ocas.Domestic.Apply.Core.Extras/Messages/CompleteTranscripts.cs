using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class CompleteTranscripts : IRequest, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
