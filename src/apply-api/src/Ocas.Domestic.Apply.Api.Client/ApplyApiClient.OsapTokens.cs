using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<OsapToken> GetOsapToken(Guid applicantId)
        {
            return Get<OsapToken>(QueryHelpers.AddQueryString(Constants.Route.OsapTokens, "applicantId", applicantId.ToString()));
        }
    }
}
