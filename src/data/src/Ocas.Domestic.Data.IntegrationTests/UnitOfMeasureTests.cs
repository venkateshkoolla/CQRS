using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class UnitOfMeasureTests : BaseTest
    {
        [Fact]
        public async Task GetUnitOfMeasures_ShouldPass()
        {
            var enResult = await Context.GetUnitOfMeasures(Locale.English);
            var frResult = await Context.GetUnitOfMeasures(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetUnitOfMeasure_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.UnitOfMeasures);

            var unitOfMeasure = await Context.GetUnitOfMeasure(result.Id, Locale.English);

            unitOfMeasure.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetUnitOfMeasure_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.UnitOfMeasures);

            var unitOfMeasure = await Context.GetUnitOfMeasure(result.Id, Locale.French);

            unitOfMeasure.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
