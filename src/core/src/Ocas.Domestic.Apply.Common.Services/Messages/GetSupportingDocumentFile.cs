using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Services.Messages
{
    public class GetSupportingDocumentFile : IRequest<BinaryDocument>, IIdentityUser
    {
        public Guid Id { get; set; }

        public IPrincipal User { get; set; }
    }
}