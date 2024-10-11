using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetApplicant : IRequest<Applicant>, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public Guid? ApplicantId { get; set; }
    }
}
