using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Services.Messages
{
    public class UpdateProgramChoices : IRequest<IList<ProgramChoice>>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public List<ProgramChoiceBase> ProgramChoices { get; set; }
        public IPrincipal User { get; set; }
    }
}
