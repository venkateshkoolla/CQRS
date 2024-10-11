using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetApplicationSummaries : IRequest<IList<ApplicationSummary>>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }
        public IPrincipal User { get; set; }
    }
}
