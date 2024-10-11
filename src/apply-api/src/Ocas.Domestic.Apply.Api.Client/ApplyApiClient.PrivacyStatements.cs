using System;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<PrivacyStatement> GetLatestPrivacyStatement()
        {
            return Get<PrivacyStatement>($"{Constants.Route.PrivacyStatements}/latest");
        }

        public Task<PrivacyStatement> GetPrivacyStatement(Guid id)
        {
            return Get<PrivacyStatement>($"{Constants.Route.PrivacyStatements}/{id}");
        }
    }
}
