using System;
using System.Threading.Tasks;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient
    {
        public Task<string> GetDiagnosticAuthorization()
        {
            return Get<string>($"{Constants.Route.Diagnostics}/authorization");
        }

        public Task<ServerTime> GetDiagnosticServerTime()
        {
            return Get<ServerTime>($"{Constants.Route.Diagnostics}/server-time");
        }

        public class ServerTime
        {
            public DateTime Utc { get; set; }
            public DateTimeOffset Local { get; set; }
        }
    }
}