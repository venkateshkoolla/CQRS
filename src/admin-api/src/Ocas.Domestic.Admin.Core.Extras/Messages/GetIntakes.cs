using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetIntakes : IRequest<IList<IntakeBrief>>, IIdentityUser
    {
        public Guid ApplicationCycleId { get; set; }
        public Guid? CollegeId { get; set; }
        public GetIntakesOptions Options { get; set; }
        public IPrincipal User { get; set; }
    }
}
