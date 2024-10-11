using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetApplicationCycles : IRequest<IList<ApplicationCycle>>, IIdentityUser
    {
        public IPrincipal User { get; set; }
    }
}
