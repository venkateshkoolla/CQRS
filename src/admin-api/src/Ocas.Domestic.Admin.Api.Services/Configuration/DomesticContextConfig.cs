using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Configuration
{
    internal class DomesticContextConfig : IDomesticContextConfig
    {
        public DomesticContextConfig(string crmConnectionString, string crmWcfServiceUrl, string crmExtrasConnectionString, int commandTimeout)
        {
            CrmConnectionString = crmConnectionString;
            CrmWcfServiceUrl = crmWcfServiceUrl;
            CrmExtrasConnectionString = crmExtrasConnectionString;
            CommandTimeout = commandTimeout;
        }

        public string CrmConnectionString { get; }
        public string CrmWcfServiceUrl { get; }
        public string CrmExtrasConnectionString { get; }
        public int CommandTimeout { get; }
    }
}
