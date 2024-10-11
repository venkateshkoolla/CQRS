using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class ReissueTranscriptRequest : IRequest<IList<TranscriptRequest>>, IIdentityUser
    {
        public Guid TranscriptRequestId { get; set; }
        public IPrincipal User { get; set; }
    }
}
