using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class PreferredCorrespondenceMethodTests : BaseTest
    {
        [Fact]
        public async Task GetPreferredCorrespondenceMethods_ShouldPass()
        {
            var enResult = await Context.GetPreferredCorrespondenceMethods(Locale.English);
            var frResult = await Context.GetPreferredCorrespondenceMethods(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetPreferredCorrespondenceMethod_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PreferredCorrespondenceMethods);
            var preferredCorrespondenceMethod = await Context.GetPreferredCorrespondenceMethod(result.Id, Locale.English);

            preferredCorrespondenceMethod.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPreferredCorrespondenceMethod_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PreferredCorrespondenceMethods);
            var preferredCorrespondenceMethod = await Context.GetPreferredCorrespondenceMethod(result.Id, Locale.French);

            preferredCorrespondenceMethod.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
