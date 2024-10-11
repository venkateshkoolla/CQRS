using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class UpdateApplicantMessage : IRequest, IIdentityUser
    {
        public Guid Id { get; set; }
        public IPrincipal User { get; set; }
    }
}
