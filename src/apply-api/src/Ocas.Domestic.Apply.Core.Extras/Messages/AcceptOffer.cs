using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class AcceptOffer : IRequest<IList<Offer>>, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public Guid OfferId { get; set; }
    }
}
