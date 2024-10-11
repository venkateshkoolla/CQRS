using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class VerifyApplicantEmailAddress : IRequest<OcasVerificationDetails>
    {
        public string EmailAddress { get; set; }
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
