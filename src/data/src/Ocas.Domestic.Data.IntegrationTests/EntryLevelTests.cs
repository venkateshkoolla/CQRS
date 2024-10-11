using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class EntryLevelTests : BaseTest
    {
        [Fact]
        public async Task GetEntryLevels_ShouldPass()
        {
            var enResult = await Context.GetEntryLevels(Locale.English);
            var frResult = await Context.GetEntryLevels(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetEntryLevel_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

            var entryLevel = await Context.GetEntryLevel(result.Id, Locale.English);

            entryLevel.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetEntryLevel_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

            var entryLevel = await Context.GetEntryLevel(result.Id, Locale.French);

            entryLevel.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
