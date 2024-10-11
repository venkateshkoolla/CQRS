using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class ProgramCodeAvailable : IRequest<bool>, IIdentityUser
    {
        public Guid CollegeApplicationCycleId { get; set; }
        public string Code { get; set; }
        public Guid CampusId { get; set; }
        public Guid DeliveryId { get; set; }
        public IPrincipal User { get; set; }
    }
}
