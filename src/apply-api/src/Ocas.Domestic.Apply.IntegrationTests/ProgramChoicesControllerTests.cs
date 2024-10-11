using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class ProgramChoicesControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _faker;

        public ProgramChoicesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateProgramChoices_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var currentApplication = await ClientFixture.CreateApplication(currentApplicant.Id);

            var programChoices = _modelFakerFixture.GetProgramChoiceBase().Generate(1);
            var programIntakes = await AlgoliaClient.GetProgramOfferings(currentApplication.ApplicationCycleId);
            programChoices[0].IntakeId = programIntakes[0].IntakeId;
            programChoices[0].EntryLevelId = programIntakes[0].ProgramValidEntryLevelIds?.Any() != true
                ? programIntakes[0].ProgramEntryLevelId
                : _faker.PickRandom(programIntakes[0].ProgramValidEntryLevelIds);

            // Act
            var result = await Client.PutProgramChoices(currentApplication.Id, programChoices);

            // Assert
            result.Should().ContainSingle();
            var actual = result.First();
            var expected = programChoices.First();
            var expectedIntake = programIntakes.First();
            actual.IntakeId.Should().Be(expected.IntakeId);
            actual.EntryLevelId.Should().Be(expected.EntryLevelId);
            actual.ApplicantId.Should().Be(currentApplicant.Id);
            actual.ApplicationId.Should().Be(currentApplication.Id);
            actual.CollegeName.Should().Be(expectedIntake.AlternateCollegeName);
            actual.ProgramCode.Should().Be(expectedIntake.ProgramCode);
            actual.IntakeStartDate.Should().Be(expectedIntake.IntakeStartDate);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetProgramChoices_ShouldPass_When_ApplicationId()
        {
            // Arrange
            var applicant = await ClientFixture.CreateNewApplicant();
            var application = await ClientFixture.CreateApplication(applicant.Id);
            var choices = await ClientFixture.CreateProgramChoices(application);

            // Act
            var result = await Client.GetProgramChoices(applicationId: application.Id);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(choices);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetProgramChoices_ShouldPass_When_ApplicantId()
        {
            // Arrange
            var applicant = await ClientFixture.CreateNewApplicant();
            var application = await ClientFixture.CreateApplication(applicant.Id);
            var choices = await ClientFixture.CreateProgramChoices(application);

            // Act
            var result = await Client.GetProgramChoices(applicantId: applicant.Id);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(choices);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetProgramChoices_ShouldPass_When_RemovedChoices()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var currentApplication = await ClientFixture.CreateApplication(currentApplicant.Id);
            var currentChoices = await ClientFixture.CreateProgramChoices(currentApplication); // Add choices

            await Client.PutProgramChoices(currentApplication.Id, new List<ProgramChoiceBase>()); // Remove choices

            // Act
            var result = await Client.GetProgramChoices(applicationId: currentApplication.Id, isRemoved: true);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(currentChoices, opts =>
                opts.Excluding(i => i.EligibleEntryLevelIds)
                .Excluding(i => i.IsActive)
                .Excluding(i => i.ModifiedOn)
                .Excluding(i => i.SupplementalFeeDescription));
            result.Should().OnlyContain(r => r.EligibleEntryLevelIds == null);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetProgramChoices_ShouldPass_When_ZeroChoices()
        {
            // Arrange
            var applicant = await ClientFixture.CreateNewApplicant();
            var application = await ClientFixture.CreateApplication(applicant.Id);

            // Act
            var result = await Client.GetProgramChoices(applicationId: application.Id);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
