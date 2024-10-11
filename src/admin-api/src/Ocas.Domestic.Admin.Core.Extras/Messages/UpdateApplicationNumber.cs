using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpdateApplicationNumber : IRequest<Application>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public string Number { get; set; }
        public IPrincipal User { get; set; }
    }
}
