using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetApplicantBriefs : IRequest<PagedResult<ApplicantBrief>>, IIdentityUser
    {
        public GetApplicantBriefOptions Params { get; set; }
        public IPrincipal User { get; set; }
    }
}
