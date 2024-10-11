using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class CreateApplicantBase : IRequest<Applicant>, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public ApplicantBase ApplicantBase { get; set; }
    }
}
