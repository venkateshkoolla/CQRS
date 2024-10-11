using Xunit;

namespace Ocas.Domestic.Coltrane.Bds.Provider.IntegrationTests
{
    [CollectionDefinition(nameof(XunitInjectionCollection))]
    public class XunitInjectionCollection
    {
        // Purposely left blank, this is used by xUnit to create instances once for the assembly
        // Could also use IClassFixture in here as well, but would be created once per class
    }
}
