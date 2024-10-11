using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class CreateTranscriptRequests : IRequest<IList<TranscriptRequest>>, IIdentityUser
    {
        public IList<TranscriptRequestBase> TranscriptRequests { get; set; }
        public IPrincipal User { get; set; }
    }
}
