using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetCities : IRequest<IList<City>>, IIdentityUser
    {
        public IPrincipal User { get; set; }
        public Guid? ProvinceId { get; set; }
    }
}
