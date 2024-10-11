using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetOffers : IRequest<IList<Offer>>, IApplicantUser
    {
        public Guid ApplicantId { get; set; }

        public IPrincipal User { get; set; }
    }
}
