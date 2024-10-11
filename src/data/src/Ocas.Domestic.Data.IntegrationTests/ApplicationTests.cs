using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ApplicationTests : BaseTest
    {
        [Fact]
        public async Task GetApplications_ShouldPass()
        {
            var results = await Context.GetApplications(TestConstants.Application.ApplicantId);

            results.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetApplication_ShouldPass()
        {
            var applications = await Context.GetApplications(TestConstants.Application.ApplicantId);
            var application = DataFakerFixture.Faker.PickRandom(applications);

            var result = await Context.GetApplication(application.Id);

            result.Should().BeEquivalentTo(application);
        }

        [Fact]
        public async Task CreateApplication_ShouldPass()
        {
            Models.Contact applicant = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());
                var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
                applicationBase.ApplicantId = applicant.Id;
                applicationBase.ModifiedBy = applicant.Username;

                var result = await Context.CreateApplication(applicationBase);

                result.Should().BeEquivalentTo(applicationBase, opt => opt
                    .Excluding(z => z.ModifiedOn)
                    .Excluding(z => z.CreatedOn));
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task TriggerDeclineEmail_ShouldPass()
        {
            // Arrange
            var applicant = await Context.GetContact(TestConstants.Application.ApplicantId);
            var applications = await Context.GetApplications(TestConstants.Application.ApplicantId);
            var application = applications.First();

            // Act
            Func<Task> action = () => Context.TriggerDeclineEmail(application.Id, applicant.Username);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task IsDuplicateApplication_ShouldPass_When_Application_IsNotDuplicated()
        {
            // Arrange
            var applications = await Context.GetApplications(TestConstants.Application.ApplicantId);
            var applicationId = DataFakerFixture.Faker.PickRandom(applications).Id;
            var application = await Context.GetApplication(applicationId);

            // Act
            var duplicated = await Context.IsDuplicateApplication(application.Id, application.ApplicationNumber);

            // Assert
            duplicated.Should().BeFalse();
        }

        [Fact]
        public async Task IsDuplicateApplication_ShouldPass_When_Application_IsDuplicated()
        {
            // Arrange
            var applications = await Context.GetApplications(TestConstants.Application.ApplicantId);
            var applicationId = DataFakerFixture.Faker.PickRandom(applications).Id;
            var application = await Context.GetApplication(applicationId);

            // Act
            var duplicated = await Context.IsDuplicateApplication(Guid.NewGuid(), application.ApplicationNumber);

            // Assert
            duplicated.Should().BeTrue();
        }
    }
}
