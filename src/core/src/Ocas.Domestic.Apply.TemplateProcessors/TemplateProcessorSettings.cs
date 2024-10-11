namespace Ocas.Domestic.Apply.TemplateProcessors
{
    public class TemplateProcessorSettings : ITemplateProcessorSettings
    {
        public TemplateProcessorSettings(string ocasEvoPdfLicenceKey, string ocasEvoPdfServiceUrl, string ocasEvoPdfServicePassword)
        {
            OcasEvoPdfLicenceKey = ocasEvoPdfLicenceKey;
            OcasEvoPdfServiceUrl = ocasEvoPdfServiceUrl;
            OcasEvoPdfServicePassword = ocasEvoPdfServicePassword;
        }

        public string OcasEvoPdfLicenceKey { get; }
        public string OcasEvoPdfServiceUrl { get; }
        public string OcasEvoPdfServicePassword { get; }
    }
}
