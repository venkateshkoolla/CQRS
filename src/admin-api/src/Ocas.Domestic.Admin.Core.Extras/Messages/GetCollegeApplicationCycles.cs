using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetCollegeApplicationCycles : IRequest<IList<CollegeApplicationCycle>>, ICollegeUser
    {
        public Guid CollegeId { get; set; }
        public IPrincipal User { get; set; }
    }
}
