using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetCollegeTransmissionHistories : IRequest<PagedResult<CollegeTransmissionHistory>>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public IPrincipal User { get; set; }
        public GetCollegeTransmissionHistoryOptions Options { get; set; }
    }
}
