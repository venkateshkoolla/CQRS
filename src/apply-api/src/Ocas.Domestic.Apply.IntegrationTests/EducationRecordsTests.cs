using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class EducationRecordsTests : BaseTest<ApplyApiClient>
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly Faker _faker;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly MonerisFixture _monerisFixture;

        public EducationRecordsTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _monerisFixture = XunitInjectionCollection.MonerisFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateEducation_ShouldPass_WhenInternational()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default, Intl");
            educationBase.ApplicantId = currentApplicant.Id;

            // Act
            var education = await Client.PostEducation(educationBase);

            // Assert
            education.Should().NotBeNull();
            education.Id.Should().NotBeEmpty();
            education.Should().BeEquivalentTo(educationBase);
            education.ModifiedBy.Should().Be(currentApplicant.Email);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateEducation_ShouldPass_WhenAcademicUpgrading()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default, AcademicUpgrading");
            educationBase.ApplicantId = currentApplicant.Id;

            // Act
            var education = await Client.PostEducation(educationBase);

            // Assert
            education.Should().NotBeNull();
            education.Id.Should().NotBeEmpty();
            education.Should().BeEquivalentTo(educationBase);
            education.ModifiedBy.Should().Be(currentApplicant.Email);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateEducation_ShouldPass_WhenCanadianCollege()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            educationBase.ApplicantId = currentApplicant.Id;

            // Act
            var education = await Client.PostEducation(educationBase);

            // Assert
            education.Should().NotBeNull();
            education.Id.Should().NotBeEmpty();
            education.Should().BeEquivalentTo(educationBase);
            education.ModifiedBy.Should().Be(currentApplicant.Email);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateEducation_ShouldPass_WhenCanadianUniversity()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,University");
            educationBase.ApplicantId = currentApplicant.Id;

            // Act
            var education = await Client.PostEducation(educationBase);

            // Assert
            education.Should().NotBeNull();
            education.Id.Should().NotBeEmpty();
            education.Should().BeEquivalentTo(educationBase);
            education.ModifiedBy.Should().Be(currentApplicant.Email);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateEducation_ShouldPass_WhenCanadianHighSchool()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,Highschool");
            educationBase.ApplicantId = currentApplicant.Id;

            // Act
            var education = await Client.PostEducation(educationBase);

            // Assert
            education.Should().NotBeNull();
            education.Id.Should().NotBeEmpty();
            education.Should().BeEquivalentTo(educationBase);
            education.ModifiedBy.Should().Be(currentApplicant.Email);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateEducation_ShouldThrow_WhenNotApplicants()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationBase = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,University");
            educationBase.ApplicantId = currentApplicant.Id;

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            Func<Task> action = () => Client.PostEducation(educationBase);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetEducations_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educations = new List<Education>
            {
                await ClientFixture.CreateEducation(currentApplicant.Id),
                await ClientFixture.CreateEducation(currentApplicant.Id),
                await ClientFixture.CreateEducation(currentApplicant.Id)
            };

            // Act
            var educationRecords = await Client.GetEducations(currentApplicant.Id);

            // Assert
            educationRecords.Should().NotBeNullOrEmpty();
            educationRecords.Should().BeEquivalentTo(educations, opts =>
                opts.Excluding(z => z.CanDelete));
            educationRecords.Should().OnlyContain(e => e.CanDelete);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetEducations_ShouldPass_WhenOneRecord()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educations = new List<Education>
            {
                await ClientFixture.CreateEducation(currentApplicant.Id)
            };

            // Act
            var educationRecords = await Client.GetEducations(currentApplicant.Id);

            // Assert
            educationRecords.Should().NotBeNull();
            educationRecords.Should().HaveSameCount(educations);
            educationRecords.Should().BeEquivalentTo(educations);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetEducations_ShouldThrow_WhenNotApplicants()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id);

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            Func<Task> action = () => Client.GetEducations(currentApplicant.Id);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateEducation_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var before = await ClientFixture.CreateEducation(currentApplicant.Id, EducationType.International);

            // Make some changes to the education record
            var changeModel = _modelFakerFixture.GetEducation().Generate("default, Intl");
            changeModel.Id = before.Id;
            changeModel.ApplicantId = before.ApplicantId;

            // Act
            var after = await Client.UpdateEducation(changeModel);

            // Assert
            after.Should().NotBeNull();
            after.Id.Should().Be(before.Id);
            after.Should().BeEquivalentTo(changeModel, opt => opt
               .Excluding(z => z.ModifiedOn)
               .Excluding(z => z.ModifiedBy));
            after.ModifiedBy.Should().Be(currentApplicant.Email);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateEducation_ShouldThrow_WhenNotApplicants()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id);

            // Make some changes to the education record
            var changeModel = _modelFakerFixture.GetEducation().Generate("default, Intl");
            changeModel.Id = education.Id;
            changeModel.ApplicantId = education.ApplicantId;

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            Func<Task> action = () => Client.UpdateEducation(changeModel);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateEducation_ShouldThrow_WhenNotAuthorized()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id);

            // Create a second applicant
            var newApplicant = await ClientFixture.CreateNewApplicant();

            // Make it look like this education is mine
            education.ApplicantId = newApplicant.Id;

            // Act
            Func<Task> action = () => Client.UpdateEducation(education);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [IntegrationTest]
        public async Task RemoveEducation_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            var educations = new List<Education>
            {
              await ClientFixture.CreateEducation(currentApplicant.Id),
              await ClientFixture.CreateEducation(currentApplicant.Id)
            };

            // Act
            var educationIdToRemove = educations.First().Id;
            await Client.RemoveEducation(educationIdToRemove);

            // Assert
            var educationsActual = await Client.GetEducations(currentApplicant.Id);
            educationsActual.Should().NotBeNullOrEmpty();
            educationsActual.Should().ContainSingle();
            educationsActual.Should().NotContain(x => x.Id == educationIdToRemove);
        }

        [Fact]
        [IntegrationTest]
        public async Task RemoveEducation_ShouldThrow_WhenDifferentApplicant()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var education = await ClientFixture.CreateEducation(currentApplicant.Id);

            // Create a second applicant
            var newApplicant = await ClientFixture.CreateNewApplicant();
            education.ApplicantId = newApplicant.Id;

            // Act
            Func<Task> action = () => Client.RemoveEducation(education.Id);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task RemoveEducation_ShouldThrow_WhenApplicationPaid()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            currentApplicant = await ClientFixture.CreateProfile(currentApplicant);

            var educations = new List<Education>
            {
                await ClientFixture.CreateEducation(currentApplicant.Id, EducationType.International),
                await ClientFixture.CreateEducation(currentApplicant.Id, EducationType.International)
            };
            var educationIdToRemove = _faker.PickRandom(educations).Id;
            var educationsExpected = await Client.GetEducations(currentApplicant.Id);
            educationsExpected.Should().OnlyContain(e => e.CanDelete);

            var application = await ClientFixture.CreateApplication(currentApplicant.Id);
            await ClientFixture.CreateProgramChoices(application);
            await Client.CompleteTranscripts(application.Id);

            await Client.GetShoppingCart(application.Id);
            var order = await Client.CreateOrder(new CreateOrderInfo { ApplicationId = application.Id });

            var token = await _monerisFixture.GetPaymentToken(TestConstants.Moneris.MasterCard);
            var payOrderInfo = new PayOrderInfo
            {
                CardHolderName = $"{currentApplicant.FirstName} {currentApplicant.LastName}",
                CardNumberToken = token,
                Csc = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(1).ToString(Constants.DateFormat.CcExpiry)
            };
            await Client.PayOrder(order.Id, payOrderInfo);

            // Act
            Func<Task> action = () => Client.RemoveEducation(educationIdToRemove);

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should().NotBeEmpty()
                    .And.Contain(x => x.Message == "Cannot remove education with paid application.");

            var educationsActual = await Client.GetEducations(currentApplicant.Id);
            educationsActual.Should().NotBeNullOrEmpty();
            educationsActual.Should().BeEquivalentTo(educationsExpected, opt => opt.Excluding(x => x.CanDelete));
            educationsActual.Should().OnlyContain(e => !e.CanDelete);
        }
    }
}
