using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class FirstGenerationApplicantTests : BaseTest
    {
        [Fact]
        public async Task GetFirstGenerationApplicants_ShouldPass()
        {
            var enResult = await Context.GetFirstGenerationApplicants(Locale.English);
            var frResult = await Context.GetFirstGenerationApplicants(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetFirstGenerationApplicant_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.FirstGenerationApplicants);
            var firstGenerationApplicant = await Context.GetFirstGenerationApplicant(result.Id, Locale.English);

            firstGenerationApplicant.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetFirstGenerationApplicant_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.FirstGenerationApplicants);
            var firstGenerationApplicant = await Context.GetFirstGenerationApplicant(result.Id, Locale.French);

            firstGenerationApplicant.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
