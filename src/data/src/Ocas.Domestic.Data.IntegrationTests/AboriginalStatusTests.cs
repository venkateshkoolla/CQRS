using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class AboriginalStatusTests : BaseTest
    {
        [Fact]
        public async Task GetAboriginalStatuses_ShouldPass()
        {
            var enResult = await Context.GetAboriginalStatuses(Locale.English);
            var frResult = await Context.GetAboriginalStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetAboriginalStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AboriginalStatuses);
            var aboriginalStatus = await Context.GetAboriginalStatus(result.Id, Locale.English);

            aboriginalStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetAboriginalStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AboriginalStatuses);
            var aboriginalStatus = await Context.GetAboriginalStatus(result.Id, Locale.French);

            aboriginalStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }

        [Fact]
        public async Task GetAboriginalStatuses_ShouldPass_WhenOther()
        {
            var enResult = await Context.GetAboriginalStatuses(Locale.English);

            var total = enResult.Count();
            var otherAboriginalStatus = enResult.Single(x => x.Code == TestConstants.AboriginalStatuses.Other);
            var otherMask = Convert.ToInt32(otherAboriginalStatus.ColtraneCode, 2);
            var numStatusesIncludingOther = enResult.Count(x => (Convert.ToInt32(x.ColtraneCode, 2) & otherMask) != 0);

            // +1 because 0000 (no aboriginal status) is not a valid code
            var expected = (total + 1) / 2;
            numStatusesIncludingOther.Should().Be(expected);
        }
    }
}
