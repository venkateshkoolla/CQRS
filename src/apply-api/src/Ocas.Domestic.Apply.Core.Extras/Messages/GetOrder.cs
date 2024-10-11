using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetOrder : IRequest<Order>, IIdentityUser
    {
        public Guid OrderId { get; set; }
        public IPrincipal User { get; set; }
    }
}
