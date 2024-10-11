using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TestTypeTests : BaseTest
    {
        [Fact]
        public async Task GetTestTypes_ShouldPass()
        {
            var results = await Context.GetTestTypes(Locale.English);
            var activeResults = await Context.GetTestTypes(Locale.English, State.Active);

            results.Should().NotBeEmpty();
            activeResults.Should().HaveSameCount(results);
        }

        [Fact]
        public async void GetTestTypes_ShouldPass_When_Inactive()
        {
            var enResult = await Context.GetTestTypes(Locale.English, State.Inactive);
            var frResult = await Context.GetTestTypes(Locale.French, State.Inactive);

            enResult.Should().NotBeEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async void GetTestTypes_ShouldPass_When_State_Null()
        {
            var enResult = await Context.GetTestTypes(Locale.English, null);
            var frResult = await Context.GetTestTypes(Locale.French, null);

            enResult.Should().NotBeEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetTestType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TestTypes);
            var testType = await Context.GetTestType(result.Id, Locale.English);

            testType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetTestType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TestTypes);
            var testType = await Context.GetTestType(result.Id, Locale.French);

            testType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
