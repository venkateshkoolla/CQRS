using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class CreateProgramChoice : IIdentityUser, IRequest<ApplicationSummary>
    {
        public Guid ApplicationId { get; set; }
        public CreateProgramChoiceRequest ProgramChoice { get; set; }
        public IPrincipal User { get; set; }
    }
}
