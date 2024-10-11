using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class BasisForAdmissionTests : BaseTest
    {
        [Fact]
        public async Task GetBasisForAdmissions_ShouldPass()
        {
            var enResult = await Context.GetBasisForAdmissions(Locale.English);
            var frResult = await Context.GetBasisForAdmissions(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetBasisForAdmission_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.BasisForAdmissions);
            var basisForAdmission = await Context.GetBasisForAdmission(result.Id, Locale.English);

            basisForAdmission.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetBasisForAdmission_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.BasisForAdmissions);
            var basisForAdmission = await Context.GetBasisForAdmission(result.Id, Locale.French);

            basisForAdmission.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
