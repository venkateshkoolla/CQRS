using System;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class ApplicantsControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _fakerFixture;

        public ApplicantsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _fakerFixture = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task AcceptLatestPrivacyStatement_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var privacyStatement = await Client.GetLatestPrivacyStatement();

            // Act
            await Client.AcceptPrivacyStatement(currentApplicant.Id, privacyStatement.Id);
            var updatedApplicant = await Client.GetCurrentApplicant();

            // Assert
            updatedApplicant.AcceptedPrivacyStatementId.Should().Be(privacyStatement.Id);
        }

        [Fact]
        [IntegrationTest]
        public async Task AcceptLatestPrivacyStatement_ShouldThrow_WhenInvalid()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            Func<Task> action = () => Client.AcceptPrivacyStatement(currentApplicant.Id, Guid.Empty);

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should()
                .Contain(x => x.Code == ErrorCodes.General.ValidationError);
        }

        [Fact]
        [IntegrationTest]
        public async Task AcceptLatestPrivacyStatement_ShouldThrow_WhenNotApplicants()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            Func<Task> action = () => Client.AcceptPrivacyStatement(currentApplicant.Id, Guid.Empty);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateCommPrefs_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var agreeToCasl = _fakerFixture.Random.Bool();

            // Act
            await Client.UpdateCommPrefs(currentApplicant.Id, agreeToCasl);
            var updatedApplicant = await Client.GetCurrentApplicant();

            // Assert
            updatedApplicant.AgreedToCasl.Should().Be(agreeToCasl);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateCommPrefs_ShouldThrow_WhenNotApplicants()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var agreeToCasl = _fakerFixture.Random.Bool();

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            Func<Task> action = () => Client.UpdateCommPrefs(currentApplicant.Id, agreeToCasl);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateEducationStatus_WhenEnrolled_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationStatusInfo = new EducationStatusInfo
            {
                EnrolledInHighSchool = true,
                GraduatedHighSchool = false,
                GraduationHighSchoolDate = _fakerFixture.Date.Future(5).ToString(Constants.DateFormat.YearMonthDashed)
            };

            // Act
            await Client.UpdateEducationStatus(currentApplicant.Id, educationStatusInfo);
            var updatedApplicant = await Client.GetCurrentApplicant();

            // Assert
            updatedApplicant.EnrolledInHighSchool.Should().BeTrue();
            updatedApplicant.GraduatedHighSchool.Should().BeNull();
            updatedApplicant.GraduationHighSchoolDate.Should().Be(educationStatusInfo.GraduationHighSchoolDate);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateEducationStatus_ShouldThrow_WhenNotApplicants()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationStatusInfo = new EducationStatusInfo
            {
                EnrolledInHighSchool = true,
                GraduatedHighSchool = false,
                GraduationHighSchoolDate = _fakerFixture.Date.Future(5).ToString(Constants.DateFormat.YearMonthDashed)
            };

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            Func<Task> action = () => Client.UpdateEducationStatus(currentApplicant.Id, educationStatusInfo);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateEducationStatus_WhenNotEnrolled_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var educationStatusInfo = new EducationStatusInfo
            {
                EnrolledInHighSchool = false,
                GraduatedHighSchool = true,
                GraduationHighSchoolDate = _fakerFixture.Date.Past(5).ToString(Constants.DateFormat.YearMonthDashed)
            };

            // Act
            await Client.UpdateEducationStatus(currentApplicant.Id, educationStatusInfo);
            var updatedApplicant = await Client.GetCurrentApplicant();

            // Assert
            updatedApplicant.EnrolledInHighSchool.Should().BeFalse();
            updatedApplicant.GraduatedHighSchool.Should().BeTrue();
            updatedApplicant.GraduationHighSchoolDate.Should().BeNull();
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateApplicant_ShouldThrow_ConflictException_When_DuplicateDetails()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var knownDetails = await Client.GetCurrentApplicant();

            var applicant = _modelFakerFixture.GetApplicant().Generate();
            applicant.FirstName = knownDetails.FirstName;
            applicant.LastName = knownDetails.LastName;
            applicant.BirthDate = knownDetails.BirthDate;

            await IdentityClientFixture.CreateApplicant(applicant);
            var testUser = await IdentityUserFixture.GetApplicantUser(applicant.Email, TestConstants.Identity.Providers.OcasApplicants.ValidPassword);
            Client.WithAccessToken(testUser.AccessToken);

            // Act
            Func<Task> action = () => Client.PostCurrentApplicant(applicant);

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should().ContainSingle(x => x.Message == "Applicant exists with same first name, last name and date of birth")
                .Which.Code.Should().Be(ErrorCodes.General.ConflictError);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateApplicant_ShouldPass()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicant().Generate();
            await IdentityClientFixture.CreateApplicant(model);
            var testUser = await IdentityUserFixture.GetApplicantUser(model.Email, TestConstants.Identity.Providers.OcasApplicants.ValidPassword);
            Client.WithAccessToken(testUser.AccessToken);

            // Act
            var currentApplicant = await Client.PostCurrentApplicant(model);

            // Assert
            currentApplicant.Should().NotBeNull();
            currentApplicant.Email.Should().Be(model.Email);
            currentApplicant.FirstName.Should().Be(model.FirstName);
            currentApplicant.LastName.Should().Be(model.LastName);
            currentApplicant.BirthDate.Should().Be(model.BirthDate);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateApplicant_ShouldPass()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var update = _modelFakerFixture.GetApplicant().Generate();
            update.Id = currentApplicant.Id;
            update.AccountNumber = currentApplicant.AccountNumber;
            update.FirstName = currentApplicant.FirstName;
            update.LastName = currentApplicant.LastName;
            update.BirthDate = currentApplicant.BirthDate;
            update.UserName = currentApplicant.UserName;
            if (update.DateOfArrival != null && update.DateOfArrival.ToDateTime() < update.BirthDate.ToDateTime())
            {
                update.DateOfArrival = update.BirthDate;
            }

            // Act
            var updatedApplicant = await Client.UpdateApplicant(currentApplicant.Id, update);

            // Assert
            updatedApplicant.Should().BeEquivalentTo(update, opts => opts
                .Excluding(x => x.AcceptedPrivacyStatementId)
                .Excluding(x => x.MailingAddress.ProvinceState)
                .Excluding(x => x.MailingAddress.Country)
                .Excluding(x => x.AccountStatusId)
                .Excluding(x => x.AgreedToCasl)
                .Excluding(x => x.EnrolledInHighSchool)
                .Excluding(x => x.GraduatedHighSchool)
                .Excluding(x => x.GraduationHighSchoolDate)
                .Excluding(x => x.LastLogin)
                .Excluding(x => x.Source));
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateApplicant_ShouldThrow_WhenNotApplicants()
        {
            // Arrange
            var currentApplicant = await ClientFixture.CreateNewApplicant();
            var update = _modelFakerFixture.GetApplicant().Generate();
            update.Id = currentApplicant.Id;

            // Act
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            Func<Task> action = () => Client.UpdateApplicant(currentApplicant.Id, update);

            // Assert
            action.Should().Throw<StatusCodeException>()
                .Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyEmail_ShouldPass_WhenUnchanged()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            var result = await Client.VerifyEmailApplicant(currentApplicant.Id, currentApplicant.Email);

            result.IsValid.Should().BeTrue();
            result.Code.Should().BeEmpty();
            result.Message.Should().BeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyEmail_ShouldPass_WhenAvaliable()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            var result = await Client.VerifyEmailApplicant(currentApplicant.Id, new Internet().Email(provider: "test.ocas.ca", uniqueSuffix: DateTime.Today.ToString("yyyyMMdd")));

            result.IsValid.Should().BeTrue();
            result.Code.Should().BeEmpty();
            result.Message.Should().BeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyEmail_ShouldPass_WhenTaken()
        {
            // Arrange
            const string email = "testapplicant2@mailinator.com";
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            var result = await Client.VerifyEmailApplicant(currentApplicant.Id, email);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be(ErrorCodes.General.ConflictVerificationError);
            result.Message.Should().Be($"Applicant exists with {email}");
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyOen_ShouldPass_WhenDefault()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            var result = await Client.VerifyOenApplicant(currentApplicant.Id, Constants.Education.DefaultOntarioEducationNumber);

            result.IsValid.Should().BeTrue();
            result.Code.Should().BeEmpty();
            result.Message.Should().BeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyOen_ShouldPass_WhenApplicantOwn()
        {
            //Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            //Act
            var result = await Client.VerifyOenApplicant(currentApplicant.Id, currentApplicant.OntarioEducationNumber);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Code.Should().BeEmpty();
            result.Message.Should().BeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyOen_ShouldPass_WhenDuplicate()
        {
            //Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var applicantOen = await Client.GetCurrentApplicant();
            var currentApplicant = await ClientFixture.CreateNewApplicant();

            //Act
            var result = await Client.VerifyOenApplicant(currentApplicant.Id, applicantOen.OntarioEducationNumber);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Code.Should().Be(ErrorCodes.General.ConflictVerificationError);
            result.Message.Should().Be($"Applicant exists with {applicantOen.OntarioEducationNumber}");
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyOen_ShouldPass_WhenNotValid()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            var result = await Client.VerifyOenApplicant(currentApplicant.Id, "123456789");

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be(ErrorCodes.General.ValidationError);
            result.Message.Should().Be("'Oen' must match checksum.");
        }

        [Fact]
        [IntegrationTest]
        public async Task VerifyOen_ShouldPass_WhenTooLong()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var currentApplicant = await Client.GetCurrentApplicant();

            // Act
            var result = await Client.VerifyOenApplicant(currentApplicant.Id, "12345678912");

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be(ErrorCodes.General.ValidationError);
            result.Message.Should().Be("'Oen' must be 9 characters in length. You entered 11 characters.");
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantLookups_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);

            // Act
            Func<Task> a = () => Client.GetLookups();

            // Assert
            await a.Should().NotThrowAsync();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicant_ShouldPass_When_OcasUser()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var applicant = await Client.GetCurrentApplicant();

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var response = await Client.GetApplicant(applicant.Id);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(applicant.Id);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateInternationalCreditAssessment_ShouldPass()
        {
            // Arrange
            var applicant = await ClientFixture.CreateNewApplicant();
            var intlCredentialAssessment = _modelFakerFixture.GetIntlCredentialAssessment().Generate();

            // Act
            await Client.UpdateInternationalCreditAssessment(applicant.Id, intlCredentialAssessment);

            // Assert
            applicant.IntlEvaluatorId.Should().BeNull();
            applicant.IntlReferenceNumber.Should().BeNullOrWhiteSpace();

            var applicantAssessment = await Client.GetCurrentApplicant();
            applicantAssessment.IntlEvaluatorId.Should().Be(intlCredentialAssessment.IntlEvaluatorId);
            applicantAssessment.IntlReferenceNumber.Should().Be(intlCredentialAssessment.IntlReferenceNumber);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateInternationalCreditAssessment_ShouldPass_When_Removed()
        {
            // Arrange
            var applicant = await ClientFixture.CreateNewApplicant();
            var intlCredentialAssessment = _modelFakerFixture.GetIntlCredentialAssessment().Generate();
            await Client.UpdateInternationalCreditAssessment(applicant.Id, intlCredentialAssessment);

            // Act
            await Client.UpdateInternationalCreditAssessment(applicant.Id, new IntlCredentialAssessment());
            var applicantRemovedAssessment = await Client.GetCurrentApplicant();

            // Assert
            applicantRemovedAssessment.IntlEvaluatorId.Should().BeNull();
            applicantRemovedAssessment.IntlReferenceNumber.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateApplicantBase_ShouldThrow_ConflictException_When_DuplicateDetails()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var knownDetails = await Client.GetCurrentApplicant();

            var applicant = _modelFakerFixture.GetApplicant().Generate();
            applicant.FirstName = knownDetails.FirstName;
            applicant.LastName = knownDetails.LastName;
            applicant.BirthDate = knownDetails.BirthDate;

            var applicantBase = new ApplicantBase
            {
                FirstName = knownDetails.FirstName,
                LastName = knownDetails.LastName,
                MiddleName = string.Empty,
                BirthDate = knownDetails.BirthDate
            };

            await IdentityClientFixture.CreateApplicant(applicant);
            var testUser = await IdentityUserFixture.GetApplicantUser(applicant.Email, TestConstants.Identity.Providers.OcasApplicants.ValidPassword);
            Client.WithAccessToken(testUser.AccessToken);

            // Act
            Func<Task> action = () => Client.PostCurrentApplicant(applicantBase);

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should().ContainSingle(x => x.Message == "Applicant exists with same first name, last name and date of birth")
                .Which.Code.Should().Be(ErrorCodes.General.ConflictError);
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateApplicantBase_ShouldPass()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicant().Generate();
            await IdentityClientFixture.CreateApplicant(model);
            var testUser = await IdentityUserFixture.GetApplicantUser(model.Email, TestConstants.Identity.Providers.OcasApplicants.ValidPassword);
            Client.WithAccessToken(testUser.AccessToken);

            var applicantBase = new ApplicantBase
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = string.Empty,
                BirthDate = model.BirthDate
            };

            // Act
            var currentApplicant = await Client.PostCurrentApplicant(applicantBase);

            // Assert
            currentApplicant.Should().NotBeNull();
            currentApplicant.FirstName.Should().Be(model.FirstName);
            currentApplicant.LastName.Should().Be(model.LastName);
            currentApplicant.MiddleName.Should().Be(model.MiddleName);
            currentApplicant.BirthDate.Should().Be(model.BirthDate);
        }
    }
}
