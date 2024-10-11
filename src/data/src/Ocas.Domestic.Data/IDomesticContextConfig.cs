namespace Ocas.Domestic.Data
{
    public interface IDomesticContextConfig
    {
        string CrmConnectionString { get; }
        string CrmWcfServiceUrl { get; }
        string CrmExtrasConnectionString { get; }
        int CommandTimeout { get; }
    }
}
