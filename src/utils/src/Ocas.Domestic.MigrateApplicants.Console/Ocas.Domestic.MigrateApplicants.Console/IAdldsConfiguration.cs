namespace Ocas.Identity.Core.Configuration
{
    public interface IAdldsConfiguration
    {
        string Domain { get; }
        string IdentityConnectionString { get; }
    }
}