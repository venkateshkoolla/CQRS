using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ApplicantMessageTests : BaseTest
    {
        [Fact]
        public async Task GetApplicantMessages_ShouldPass_When_ByApplicantId()
        {
            var options = new GetApplicantMessageOptions
            {
                ApplicantId = TestConstants.ApplicantMessage.ApplicantId
            };

            var enResult = await Context.GetApplicantMessages(options, Locale.English);
            var frResult = await Context.GetApplicantMessages(options, Locale.French);

            enResult.Should().NotBeEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetApplicantMessages_ShouldPass_When_ByApplicantId_WithCreatedDate()
        {
            var options = new GetApplicantMessageOptions
            {
                ApplicantId = TestConstants.ApplicantMessage.ApplicantId
            };
            var allResults = await Context.GetApplicantMessages(options, Locale.English);

            options.CreatedOn = DataFakerFixture.Faker.PickRandom(allResults).CreatedOn;
            var expected = allResults.Where(x => x.CreatedOn >= options.CreatedOn);

            var result = await Context.GetApplicantMessages(options, Locale.English);

            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetApplicantMessage_ShouldPass()
        {
            var options = new GetApplicantMessageOptions
            {
                ApplicantId = TestConstants.ApplicantMessage.ApplicantId
            };

            var results = await Context.GetApplicantMessages(options, Locale.English);
            var message = DataFakerFixture.Faker.PickRandom(results);

            var enResult = await Context.GetApplicantMessage(message.Id, Locale.English);
            var frResult = await Context.GetApplicantMessage(message.Id, Locale.French);

            enResult.Should().BeEquivalentTo(message);
            frResult.Should().BeEquivalentTo(message, opt =>
                opt.Excluding(z => z.LocalizedSubject)
                    .Excluding(z => z.LocalizedText));
        }

        [Fact]
        public async Task UpdateApplicantMessage_ShouldPass()
        {
            var options = new GetApplicantMessageOptions
            {
                ApplicantId = TestConstants.ApplicantMessage.ApplicantId
            };

            var results = await Context.GetApplicantMessages(options, Locale.English);
            var message = DataFakerFixture.Faker.PickRandom(results);

            message.HasRead = message.HasRead.HasValue ? !message.HasRead.Value : true;
            var updatedMessage = await Context.UpdateApplicantMessage(message, Locale.English);

            updatedMessage.Should().BeEquivalentTo(message);
        }
    }
}
