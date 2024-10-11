using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetOsapToken : IRequest<OsapToken>, IApplicantUser
    {
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
