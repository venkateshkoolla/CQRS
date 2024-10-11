using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class UpdateCommPreferences : IRequest, IApplicantUser
    {
        public IPrincipal User { get; set; }
        public Guid ApplicantId { get; set; }
        public bool AgreedToCasl { get; set; }
    }
}
