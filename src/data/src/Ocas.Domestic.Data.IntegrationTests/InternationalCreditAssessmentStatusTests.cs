using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class InternationalCreditAssessmentStatusTests : BaseTest
    {
        [Fact]
        public async Task GetInternationalCreditAssessmentStatuses_ShouldPass()
        {
            var enResult = await Context.GetInternationalCreditAssessmentStatuses(Locale.English);
            var frResult = await Context.GetInternationalCreditAssessmentStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetInternationalCreditAssessmentStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.InternationalCreditAssessmentStatuses);
            var internationalCreditAssessmentStatus = await Context.GetInternationalCreditAssessmentStatus(result.Id, Locale.English);

            internationalCreditAssessmentStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetInternationalCreditAssessmentStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.InternationalCreditAssessmentStatuses);
            var internationalCreditAssessmentStatus = await Context.GetInternationalCreditAssessmentStatus(result.Id, Locale.French);

            internationalCreditAssessmentStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
