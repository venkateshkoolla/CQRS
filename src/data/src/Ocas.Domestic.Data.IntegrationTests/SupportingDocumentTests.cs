using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class SupportingDocumentTests : BaseTest
    {
        [Fact]
        public async Task GetSupportingDocument_ShouldPass()
        {
            var result = await Context.GetSupportingDocument(TestConstants.SupportingDocuments.Id);

            result.Should().BeOfType<SupportingDocument>()
                .And.NotBeNull();
        }

        [Fact]
        public async Task GetSupportingDocuments_ShouldPass()
        {
            var results = await Context.GetSupportingDocuments(TestConstants.SupportingDocuments.ApplicantId);

            results.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ApplicantId == TestConstants.SupportingDocuments.ApplicantId);
        }
    }
}
