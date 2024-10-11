using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class VerifyProfile : IRequest, IApplicantUser
    {
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
