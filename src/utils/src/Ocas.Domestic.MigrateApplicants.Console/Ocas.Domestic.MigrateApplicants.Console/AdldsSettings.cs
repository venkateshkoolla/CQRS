namespace Ocas.Identity.Core.Configuration
{
    public class AdldsSettings : IAdldsConfiguration
    {
        public string Domain { get; set; }
        public string IdentityConnectionString { get; set; }
    }
}