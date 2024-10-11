using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetIntakesReport : IRequest<BinaryDocument>, ICollegeUser
    {
        public Guid ApplicationCycleId { get; set; }
        public Guid CollegeId { get; set; }
        public GetIntakesOptions Params { get; set; }
        public IPrincipal User { get; set; }
    }
}
