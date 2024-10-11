using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetCollegeTemplate : IRequest<CollegeTemplate>, IIdentityUser
    {
        public Guid CollegeId { get; set; }
        public CollegeTemplateKey Key { get; set; }
        public IPrincipal User { get; set; }
    }
}
