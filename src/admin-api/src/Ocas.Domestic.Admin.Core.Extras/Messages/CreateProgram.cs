using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class CreateProgram : IRequest<Program>, IIdentityUser
    {
        public ProgramBase Program { get; set; }
        public IPrincipal User { get; set; }
    }
}
