using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Services.Messages
{
    public class CreateOrder : IRequest<Order>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public bool IsOfflinePayment { get; set; }
        public IPrincipal User { get; set; }
    }
}
