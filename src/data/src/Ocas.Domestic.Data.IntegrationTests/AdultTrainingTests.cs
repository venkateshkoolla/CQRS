using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class AdultTrainingTests : BaseTest
    {
        [Fact]
        public async Task GetAdultTrainings_ShouldPass()
        {
            var enResult = await Context.GetAdultTrainings(Locale.English);
            var frResult = await Context.GetAdultTrainings(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetAdultTraining_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AdultTrainings);
            var adultTraining = await Context.GetAdultTraining(result.Id, Locale.English);

            adultTraining.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetAdultTraining_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AdultTrainings);
            var adultTraining = await Context.GetAdultTraining(result.Id, Locale.French);

            adultTraining.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
