using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetLookups : IRequest<AllLookups>, IIdentityUser
    {
        public string Filter { get; set; }
        public IPrincipal User { get; set; }
    }
}
