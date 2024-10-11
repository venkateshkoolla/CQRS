using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class GenderTests : BaseTest
    {
        [Fact]
        public async Task GetGenders_ShouldPass()
        {
            var enResult = await Context.GetGenders(Locale.English);
            var frResult = await Context.GetGenders(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetGender_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Genders);
            var gender = await Context.GetGender(result.Id, Locale.English);

            gender.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetGender_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Genders);
            var gender = await Context.GetGender(result.Id, Locale.French);

            gender.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
