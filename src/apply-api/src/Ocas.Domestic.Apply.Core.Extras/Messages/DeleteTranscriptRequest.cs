using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class DeleteTranscriptRequest : IRequest, IIdentityUser
    {
        public Guid TranscriptRequestId { get; set; }
        public IPrincipal User { get; set; }
    }
}
