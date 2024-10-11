using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetOfferHistories : IRequest<IList<OfferHistory>>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public IPrincipal User { get; set; }
    }
}
