using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetProgramBriefs : IRequest<IList<ProgramBrief>>, ICollegeUser
    {
        public Guid ApplicationCycleId { get; set; }
        public Guid CollegeId { get; set; }
        public GetProgramBriefOptions Params { get; set; }
        public IPrincipal User { get; set; }
    }
}
