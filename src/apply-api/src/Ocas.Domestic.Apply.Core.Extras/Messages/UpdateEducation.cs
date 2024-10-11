using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class UpdateEducation : IRequest<Education>, IApplicantUser
    {
        public IPrincipal User { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid EducationId { get; set; }
        public Education Education { get; set; }
    }
}
