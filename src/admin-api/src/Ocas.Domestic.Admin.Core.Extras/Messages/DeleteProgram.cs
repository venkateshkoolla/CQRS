using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class DeleteProgram : IRequest, IIdentityUser
    {
        public Guid ProgramId { get; set; }
        public IPrincipal User { get; set; }
    }
}
