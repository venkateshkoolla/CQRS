using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpdateIntakeAvailability : IRequest, IIdentityUser
    {
        public IList<Guid> IntakeIds { get; set; }
        public Guid AvailabilityId { get; set; }
        public IPrincipal User { get; set; }
    }
}
