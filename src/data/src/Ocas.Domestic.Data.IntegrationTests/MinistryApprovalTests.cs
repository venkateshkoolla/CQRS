using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class MinistryApprovalTests : BaseTest
    {
        [Fact]
        public async Task GetMinistryApprovals_ShouldPass()
        {
            var enResult = await Context.GetMinistryApprovals(Locale.English);
            var frResult = await Context.GetMinistryApprovals(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetMinistryApproval_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.MinistryApprovals);

            var ministryApproval = await Context.GetMinistryApproval(result.Id, Locale.English);

            ministryApproval.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetMinistryApproval_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.MinistryApprovals);

            var ministryApproval = await Context.GetMinistryApproval(result.Id, Locale.French);

            ministryApproval.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
