using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetApplications : IApplicantUser, IRequest<IList<Application>>
    {
        public Guid ApplicantId { get; set; }

        public IPrincipal User { get; set; }
    }
}
