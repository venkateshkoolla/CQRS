﻿using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class PurgeLookups : IRequest, IIdentityUser
    {
        public string Filter { get; set; }
        public IPrincipal User { get; set; }
    }
}
