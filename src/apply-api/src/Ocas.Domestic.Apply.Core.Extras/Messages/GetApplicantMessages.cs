using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetApplicantMessages : IRequest<IList<ApplicantMessage>>, IApplicantUser
    {
        public DateTime? After { get; set; }
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
