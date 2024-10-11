using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class DocumentPrintTests : BaseTest
    {
        [Fact]
        public async Task GetDocumentPrints_ShouldPass()
        {
            var result = await Context.GetDocumentPrints();
            result.Should().NotBeNullOrEmpty();
        }
    }
}
