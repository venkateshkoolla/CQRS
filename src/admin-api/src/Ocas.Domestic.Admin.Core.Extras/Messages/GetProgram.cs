using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetProgram : IRequest<Program>, IIdentityUser
    {
        public Guid ProgramId { get; set; }
        public IPrincipal User { get; set; }
    }
}
