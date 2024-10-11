using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpdateApplicationEffectiveDate : IRequest<Application>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public string EffectiveDate { get; set; }
        public IPrincipal User { get; set; }
    }
}
