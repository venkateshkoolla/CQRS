using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<IList<ApplicationCycle>> GetApplicationCycles()
        {
            return Get<IList<ApplicationCycle>>(Constants.Route.ApplicationCycles);
        }
    }
}
