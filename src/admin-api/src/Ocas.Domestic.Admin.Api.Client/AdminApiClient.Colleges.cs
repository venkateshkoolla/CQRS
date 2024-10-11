using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    public partial class AdminApiClient
    {
        public Task<IList<College>> GetColleges()
        {
            return Get<IList<College>>(Constants.Route.Colleges);
        }
    }
}
