using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class PrivacyStatementTests : BaseTest
    {
        [Fact]
        public async Task GetPrivacyStatements_ShouldPass()
        {
            var enResult = await Context.GetPrivacyStatements(Locale.English);
            var frResult = await Context.GetPrivacyStatements(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetLatestApplicantPrivacyStatement_ShouldPass()
        {
            var enResult = await Context.GetLatestApplicantPrivacyStatement(Locale.English);
            var frResult = await Context.GetLatestApplicantPrivacyStatement(Locale.French);

            enResult.Id.Should().Be(frResult.Id);
            frResult.Id.Should().Be(frResult.Id);

            enResult.Category.Should().Be(PrivacyStatementCategory.ApplicantPrivacyStatement);
            enResult.Content.Should().NotBeEmpty();
            enResult.EffectiveDate.Should().BeBefore(DateTime.UtcNow);
            enResult.Version.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetPrivacyStatement_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PrivacyStatements);
            var privacyStatement = await Context.GetPrivacyStatement(result.Id, Locale.English);

            privacyStatement.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPrivacyStatement_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PrivacyStatements);
            var privacyStatement = await Context.GetPrivacyStatement(result.Id, Locale.French);

            privacyStatement.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName).Excluding(p => p.Content));
        }
    }
}
