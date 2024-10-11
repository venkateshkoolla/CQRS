using System;
using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetFinancialTransactions : IRequest<IList<FinancialTransaction>>, IIdentityUser
    {
        public Guid ApplicationId { get; set; }

        public IPrincipal User { get; set; }
    }
}
