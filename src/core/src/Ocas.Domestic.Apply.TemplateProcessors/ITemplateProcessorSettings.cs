namespace Ocas.Domestic.Apply.TemplateProcessors
{
    public interface ITemplateProcessorSettings
    {
        string OcasEvoPdfLicenceKey { get; }
        string OcasEvoPdfServiceUrl { get; }
        string OcasEvoPdfServicePassword { get; }
    }
}
