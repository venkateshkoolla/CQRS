using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OfferStudyMethodTests : BaseTest
    {
        [Fact]
        public async Task GetOfferStudyMethods_ShouldPass()
        {
            var enResult = await Context.GetOfferStudyMethods(Locale.English);
            var frResult = await Context.GetOfferStudyMethods(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetOfferStudyMethod_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferStudyMethods);
            var offerStudyMethod = await Context.GetOfferStudyMethod(result.Id, Locale.English);

            offerStudyMethod.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetOfferStudyMethod_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferStudyMethods);
            var offerStudyMethod = await Context.GetOfferStudyMethod(result.Id, Locale.French);

            offerStudyMethod.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
