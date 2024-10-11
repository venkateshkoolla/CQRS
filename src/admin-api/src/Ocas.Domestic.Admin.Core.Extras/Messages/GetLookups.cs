using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetLookups : IRequest<AllLookups>, IIdentityUser
    {
        public string Filter { get; set; }
        public IPrincipal User { get; set; }
    }
}
