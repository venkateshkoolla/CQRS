using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class MessagesControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly Faker _faker;

        public MessagesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantMessages_ShouldPass()
        {
            // Arrange
            var testUser = await IdentityUserFixture.GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantWithMesssages, TestConstants.Identity.Providers.OcasApplicants.ApplicantWithMesssagesPw);
            Client.WithAccessToken(testUser.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            var applicantMessages = await Client.GetApplicantMessages(currentApplicant.Id);

            // Assert
            applicantMessages.Should().NotBeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantMessages_ShouldPass_When_AfterDate()
        {
            // Arrange
            var testUser = await IdentityUserFixture.GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantWithMesssages, TestConstants.Identity.Providers.OcasApplicants.ApplicantWithMesssagesPw);
            Client.WithAccessToken(testUser.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();
            var allMessages = await Client.GetApplicantMessages(currentApplicant.Id);
            var after = _faker.PickRandom(allMessages).CreatedOn;
            var expected = allMessages.Where(m => m.CreatedOn >= after);

            // Act
            var applicantMessages = await Client.GetApplicantMessages(currentApplicant.Id, after);

            // Assert
            applicantMessages.Should().NotBeEmpty();
            applicantMessages.Should().BeEquivalentTo(expected);
        }

        [Fact]
        [IntegrationTest]
        public async Task ReadApplicantMessage_ShouldPass()
        {
            // Arrange - Creating an international education, creates a message
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            await ClientFixture.CreateEducation(currentApplicant.Id, EducationType.International);

            var count = 1;
            ApplicantMessage messageToRead = null;
            do
            {
                Thread.Sleep(2000 * count);
                var messages = await Client.GetApplicantMessages(currentApplicant.Id);
                messageToRead = messages.FirstOrDefault();
            } while (++count <= 4 || messageToRead == null);

            if (messageToRead == null) Assert.True(false, "No message to read");

            // Act
            await Client.ReadApplicantMessage(messageToRead.Id);

            // Assert
            var allMessages = await Client.GetApplicantMessages(currentApplicant.Id);
            var readMessage = allMessages.FirstOrDefault(m => m.Id == messageToRead.Id);

            readMessage.Should().NotBeNull();
            readMessage.Read.Should().BeTrue();
        }
    }
}
