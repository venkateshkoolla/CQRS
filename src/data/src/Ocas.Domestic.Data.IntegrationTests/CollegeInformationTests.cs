using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CollegeInformationTests : BaseTest
    {
        [Fact]
        public async Task GetCollegeInformations_ShouldPass()
        {
            var enResult = await Context.GetCollegeInformations(Locale.English);
            var frResult = await Context.GetCollegeInformations(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCollegeInformation_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CollegeInformations);
            var collegeInformation = await Context.GetCollegeInformation(result.Id, Locale.English);

            collegeInformation.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCollegeInformation_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CollegeInformations);
            var collegeInformation = await Context.GetCollegeInformation(result.Id, Locale.French);

            collegeInformation.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedWelcomeText));
        }
    }
}
