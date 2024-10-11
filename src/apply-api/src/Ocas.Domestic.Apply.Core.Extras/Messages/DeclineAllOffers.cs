using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class DeclineAllOffers : IRequest<IList<Offer>>, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public Guid ApplicationId { get; set; }
        public bool? IncludeAccepted { get; set; }
    }
}
