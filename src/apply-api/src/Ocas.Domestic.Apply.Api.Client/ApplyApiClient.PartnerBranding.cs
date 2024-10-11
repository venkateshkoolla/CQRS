using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<PartnerBranding> GetPartnerBranding(string code)
        {
            return Get<PartnerBranding>($"{Constants.Route.PartnerBranding}/{code}");
        }
    }
}
