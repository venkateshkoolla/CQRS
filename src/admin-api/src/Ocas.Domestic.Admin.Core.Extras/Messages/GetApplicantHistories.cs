using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetApplicantHistories : IRequest<PagedResult<ApplicantHistory>>, IApplicantUser
    {
        public Guid ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public GetApplicantHistoryOptions Options { get; set; }
        public IPrincipal User { get; set; }
    }
}
