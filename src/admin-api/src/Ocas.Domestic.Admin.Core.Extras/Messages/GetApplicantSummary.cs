using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetApplicantSummary : IRequest<ApplicantSummary>, IApplicantUser
    {
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
