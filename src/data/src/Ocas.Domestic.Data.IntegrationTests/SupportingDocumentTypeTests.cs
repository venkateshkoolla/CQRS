using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class SupportingDocumentTypeTests : BaseTest
    {
        [Fact]
        public async Task GetSupportingDocumentTypes_ShouldPass()
        {
            var enResult = await Context.GetSupportingDocumentTypes(Locale.English);
            var frResult = await Context.GetSupportingDocumentTypes(Locale.French);

            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetSupportingDocumentType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.SupportingDocumentTypes);
            var supportingDocumentType = await Context.GetSupportingDocumentType(result.Id, Locale.English);

            supportingDocumentType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetSupportingDocumentType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.SupportingDocumentTypes);
            var supportingDocumentType = await Context.GetSupportingDocumentType(result.Id, Locale.French);

            supportingDocumentType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
