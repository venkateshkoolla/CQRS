using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetUniversities : IRequest<IList<University>>, IIdentityUser
    {
        public IPrincipal User { get; set; }
    }
}
