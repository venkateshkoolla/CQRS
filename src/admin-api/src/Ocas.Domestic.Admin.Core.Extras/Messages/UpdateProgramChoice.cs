using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpdateProgramChoice : IRequest<ProgramChoice>, IIdentityUser
    {
        public Guid ProgramChoiceId { get; set; }
        public IPrincipal User { get; set; }
        public string EffectiveDate { get; set; }
    }
}
