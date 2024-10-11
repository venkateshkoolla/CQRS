using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetSpecialCodes : IRequest<PagedResult<SpecialCode>>, IIdentityUser
    {
        public Guid CollegeApplicationCycleId { get; set; }
        public GetSpecialCodeOptions Params { get; set; }
        public IPrincipal User { get; set; }
    }
}
