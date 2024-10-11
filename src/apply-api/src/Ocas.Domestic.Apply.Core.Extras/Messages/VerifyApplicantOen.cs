using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class VerifyApplicantOen : IRequest<OcasVerificationDetails>, IIdentityUser
    {
        public string Oen { get; set; }
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
