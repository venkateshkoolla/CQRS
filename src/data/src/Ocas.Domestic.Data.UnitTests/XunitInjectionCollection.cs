using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.UnitTests
{
    [CollectionDefinition(nameof(XunitInjectionCollection))]
    public class XunitInjectionCollection
        : ICollectionFixture<DataFakerFixture>
    {
        // Purposely left blank, this is used by xUnit to create instances once for the assembly
        // Could also use IClassFixture in here as well, but would be created once per class
    }
}
