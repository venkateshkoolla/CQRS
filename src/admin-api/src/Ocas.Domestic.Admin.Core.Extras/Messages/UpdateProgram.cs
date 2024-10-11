using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpdateProgram : IRequest<Program>, IIdentityUser
    {
        public Guid ProgramId { get; set; }
        public Program Program { get; set; }
        public IPrincipal User { get; set; }
    }
}
