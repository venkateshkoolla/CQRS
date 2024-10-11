using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetSpecialCode : IRequest<SpecialCode>, IIdentityUser
    {
        public Guid CollegeApplicationCycleId { get; set; }
        public string SpecialCode { get; set; }
        public IPrincipal User { get; set; }
    }
}
