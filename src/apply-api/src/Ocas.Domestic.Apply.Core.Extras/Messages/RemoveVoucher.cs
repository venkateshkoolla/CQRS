using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class RemoveVoucher : IRequest, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public string Code { get; set; }
        public IPrincipal User { get; set; }
    }
}
