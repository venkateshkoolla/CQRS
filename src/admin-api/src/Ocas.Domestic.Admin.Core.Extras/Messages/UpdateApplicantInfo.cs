using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpdateApplicantInfo : IRequest<ApplicantUpdateInfo>, IApplicantUser
    {
        public Guid ApplicantId { get; set; }
        public ApplicantUpdateInfo ApplicantUpdateInfo { get; set; }
        public IPrincipal User { get; set; }
    }
}
