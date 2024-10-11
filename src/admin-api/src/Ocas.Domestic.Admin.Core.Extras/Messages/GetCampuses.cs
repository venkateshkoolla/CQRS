using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetCampuses : IRequest<IList<Campus>>, ICollegeUser
    {
        public IPrincipal User { get; set; }
        public Guid CollegeId { get; set; }
    }
}
