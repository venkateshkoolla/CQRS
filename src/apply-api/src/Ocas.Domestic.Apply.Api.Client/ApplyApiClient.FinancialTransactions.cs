using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<FinancialTransaction>> GetFinancialTransactions(Guid applicationId)
        {
            return Get<IList<FinancialTransaction>>(QueryHelpers.AddQueryString(Constants.Route.FinancialTransactions, "applicationId", applicationId.ToString()));
        }
    }
}