using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetIntakeApplicants : IRequest<PagedResult<IntakeApplicant>>, IIdentityUser
    {
        public Guid IntakeId { get; set; }
        public GetIntakeApplicantOptions Params { get; set; }
        public IPrincipal User { get; set; }
    }
}
