using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Client;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Admin.TestFramework;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class ApplicantsControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly ModelFakerFixture _modelFaker;
        private readonly Faker _faker;

        public ApplicantsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantBriefs_ShouldPass_AsOcasUser()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();
            var education = await ApplyClientFixture.CreateEducation(applicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var highSchool = _modelFaker.AllAdminLookups.HighSchools.FirstOrDefault(h => h.Mident == "890332") //Acton District High School
                ?? throw new Exception("Highschool with mident 890332 not found");
            education.InstituteId = highSchool.Id;
            await ApplyApiClient.UpdateEducation(education);
            await ApplyClientFixture.CreateApplication(applicant.Id);

            // Arrange
            var options = new GetApplicantBriefOptions
            {
                AccountNumber = applicant.AccountNumber
            };

            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);

            // Act
            var results = await Client.GetApplicantBriefs(options);

            // Assert
            results.Should().BeOfType<PagedResult<ApplicantBrief>>();
            results.TotalCount.Should().Be(1);
            results.Items.Should().OnlyContain(x => x.AccountNumber == applicant.AccountNumber);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantHistories_ShouldPass()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();
            await ApplyClientFixture.CreateEducation(applicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            await ApplyClientFixture.CreateApplication(applicant.Id);

            await Task.Delay(5000);
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var applicantHistories = await Client.GetApplicantHistories(applicant.Id, null, new GetApplicantHistoryOptions());

            // Assert
            applicantHistories.Items.Should().NotBeNullOrEmpty()
                .And.HaveCountGreaterThan(0);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantHistories_ShouldPass_With_Filters()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();
            await ApplyClientFixture.CreateEducation(applicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var application = await ApplyClientFixture.CreateApplication(applicant.Id);

            await Task.Delay(5000);
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var applicantHistories = await Client.GetApplicantHistories(applicant.Id, application.Id, new GetApplicantHistoryOptions { FromDate = DateTime.Now.AddDays(-1).AsUtc().ToStringOrDefault(), ToDate = DateTime.Now.AsUtc().ToStringOrDefault() });

            // Assert
            applicantHistories.Items.Should().NotBeNullOrEmpty()
               .And.HaveCountGreaterThan(0);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantSummary_ShouldPass_AsCollegeUser()
        {
            // Arrange
            var seneCollegeId = _modelFaker.AllAdminLookups.Colleges.First(c => c.Code == "SENE").Id;
            var applicant = await ApplyClientFixture.CreateNewApplicant();

            var eduHighschool = await ApplyClientFixture.CreateEducation(applicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            eduHighschool.InstituteId = _faker.PickRandom(_modelFaker.AllAdminLookups.HighSchools.Where(h => h.HasEtms && h.TranscriptFee == 0)).Id;
            eduHighschool = await ApplyApiClient.UpdateEducation(eduHighschool);

            var eduUniversity = await ApplyClientFixture.CreateEducation(applicant.Id, Apply.Core.Enums.EducationType.CanadianUniversity, true);
            eduUniversity.InstituteId = _faker.PickRandom(_modelFaker.AllAdminLookups.Universities.Where(h => h.HasEtms && h.TranscriptFee == 0)).Id;
            eduUniversity = await ApplyApiClient.UpdateEducation(eduUniversity);

            var application = await ApplyClientFixture.CreateApplication(applicant.Id);
            await ApplyClientFixture.CreateProgramChoice(application, seneCollegeId);

            var trHighSchool = _modelFaker.GetTranscriptRequestBase().Generate(1, "Highschool");
            trHighSchool.ForEach(x => x.ApplicationId = application.Id);
            trHighSchool.First().FromInstituteId = eduHighschool.InstituteId.Value;
            await ApplyApiClient.CreateTranscriptRequests(trHighSchool);

            var trUniversity = _modelFaker.GetTranscriptRequestBase().Generate(1, "University");
            trUniversity.ForEach(x => x.ApplicationId = application.Id);
            trUniversity.First().FromInstituteId = eduUniversity.InstituteId.Value;
            trUniversity.First().ToInstituteId = seneCollegeId;
            await ApplyApiClient.CreateTranscriptRequests(trUniversity);

            var academicRecordBase = _modelFaker.GetAcademicRecordBase().Generate();
            academicRecordBase.ApplicantId = applicant.Id;
            await Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken).UpsertAcademicRecord(applicant.Id, academicRecordBase);

            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);

            // Act
            var applicantSummary = await Client.GetApplicantSummary(applicant.Id);

            // Assert
            applicantSummary.Should().NotBeNull().And.BeOfType<ApplicantSummary>();

            applicantSummary.Applicant.Should().BeEquivalentTo(applicant, opt =>
                opt.Excluding(y => y.Source).Excluding(y => y.AccountStatusId).Excluding(y => y.OntarioEducationNumber)
                .Excluding(y => y.EnrolledInHighSchool).Excluding(y => y.GraduatedHighSchool).Excluding(y => y.GraduationHighSchoolDate));
            applicantSummary.Applicant.Source.Should().BeEmpty();
            applicantSummary.Applicant.AccountStatusId.Should().BeEmpty();

            applicantSummary.AcademicRecord.Should().BeEquivalentTo(academicRecordBase, opt =>
                opt.Excluding(z => z.SchoolId)
                .ExcludingMissingMembers());

            applicantSummary.Educations.Should().NotBeNullOrEmpty().And.HaveCount(2);
            applicantSummary.Educations.Should().BeEquivalentTo(new List<Education> { eduHighschool, eduUniversity }, opt =>
                opt.Excluding(y => y.HasTranscripts).Excluding(y => y.CanDelete).Excluding(y => y.ModifiedOn));

            applicantSummary.OntarioStudentCourseCredits.Should().NotBeNull().And.BeOfType<List<OntarioStudentCourseCredit>>();

            applicantSummary.ApplicationSummaries.Should().ContainSingle();
            var applicationSummary = applicantSummary.ApplicationSummaries.First();

            applicationSummary.ProgramChoices.Should().OnlyContain(c => c.CollegeId == seneCollegeId);
            applicationSummary.TranscriptRequests.Should().OnlyContain(t => t.ToInstituteId == seneCollegeId || t.ToInstituteId == null);
            applicationSummary.ShoppingCartDetails.Should().BeNull();
            applicationSummary.Offers.Should().BeEmpty(); // No offers exist and no easy way to add but filters to college id
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantSummary_ShouldPass_AsHsUser()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();

            var eduStatus = new EducationStatusInfo
            {
                EnrolledInHighSchool = true,
                GraduatedHighSchool = false,
                GraduationHighSchoolDate = DateTime.UtcNow.AddYears(3).ToString(Constants.DateFormat.YearMonthDashed)
            };
            await ApplyApiClient.UpdateEducationStatus(applicant.Id, eduStatus);

            var education = await ApplyClientFixture.CreateEducation(applicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var highSchool = _modelFaker.AllAdminLookups.HighSchools.FirstOrDefault(h => h.Mident == "890332") //Acton District High School
                ?? throw new Exception("Highschool with mident 890332 not found");
            education.InstituteId = highSchool.Id;
            education.CurrentlyAttending = true;
            education = await ApplyApiClient.UpdateEducation(education);
            var application = await ApplyClientFixture.CreateApplication(applicant.Id);
            await ApplyClientFixture.CreateProgramChoices(application, 3);

            Client.WithAccessToken(_identityUserFixture.ActonHsUser.AccessToken);

            // Act
            var applicantSummary = await Client.GetApplicantSummary(applicant.Id);

            // Assert
            applicantSummary.Should().NotBeNull().And.BeOfType<ApplicantSummary>();

            applicantSummary.Applicant.Should().BeEquivalentTo(applicant, opt =>
                opt.Excluding(y => y.Source).Excluding(y => y.AccountStatusId).Excluding(y => y.OntarioEducationNumber)
                .Excluding(y => y.EnrolledInHighSchool).Excluding(y => y.GraduatedHighSchool).Excluding(y => y.GraduationHighSchoolDate));
            applicantSummary.Applicant.Source.Should().BeEmpty();
            applicantSummary.Applicant.AccountStatusId.Should().BeEmpty();

            applicantSummary.AcademicRecord.Should().BeNull();

            applicantSummary.Educations.Should().NotBeNullOrEmpty().And.ContainSingle();
            applicantSummary.Educations.Should().BeEquivalentTo(new List<Education> { education }, opt =>
                opt.Excluding(y => y.HasTranscripts).Excluding(y => y.CanDelete).Excluding(y => y.ModifiedOn));

            applicantSummary.ApplicationSummaries.Should().ContainSingle();
            var applicationSummary = applicantSummary.ApplicationSummaries.First();

            applicationSummary.ProgramChoices.Should().HaveCount(3);
            applicationSummary.TranscriptRequests.Should().BeNull();
            applicationSummary.ShoppingCartDetails.Should().BeNull();
            applicationSummary.Offers.Should().BeNull();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicantSummary_ShouldPass_AsHsBoardUser()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();

            var eduStatus = new Apply.Models.EducationStatusInfo
            {
                EnrolledInHighSchool = true,
                GraduatedHighSchool = false,
                GraduationHighSchoolDate = DateTime.UtcNow.AddYears(3).ToString(Constants.DateFormat.YearMonthDashed)
            };
            await ApplyApiClient.UpdateEducationStatus(applicant.Id, eduStatus);

            var education = await ApplyClientFixture.CreateEducation(applicant.Id, Apply.Core.Enums.EducationType.CanadianHighSchool, true);
            var highSchool = _modelFaker.AllAdminLookups.HighSchools.FirstOrDefault(h => h.Mident == "890332") //Acton District High School
                ?? throw new Exception("Highschool with mident 890332 not found");
            education.InstituteId = highSchool.Id;
            education.CurrentlyAttending = true;
            education = await ApplyApiClient.UpdateEducation(education);
            var application = await ApplyClientFixture.CreateApplication(applicant.Id);
            await ApplyClientFixture.CreateProgramChoices(application, 3);

            Client.WithAccessToken(_identityUserFixture.HaltonBoardUser.AccessToken);

            // Act
            var applicantSummary = await Client.GetApplicantSummary(applicant.Id);

            // Assert
            applicantSummary.Should().NotBeNull().And.BeOfType<ApplicantSummary>();

            applicantSummary.Applicant.Should().BeEquivalentTo(applicant, opt =>
                opt.Excluding(y => y.Source).Excluding(y => y.AccountStatusId).Excluding(y => y.OntarioEducationNumber)
                .Excluding(y => y.EnrolledInHighSchool).Excluding(y => y.GraduatedHighSchool).Excluding(y => y.GraduationHighSchoolDate));
            applicantSummary.Applicant.Source.Should().BeEmpty();
            applicantSummary.Applicant.AccountStatusId.Should().BeEmpty();

            applicantSummary.AcademicRecord.Should().BeNull();

            applicantSummary.Educations.Should().NotBeNullOrEmpty().And.ContainSingle();
            applicantSummary.Educations.Should().BeEquivalentTo(new List<Education> { education }, opt =>
                opt.Excluding(y => y.HasTranscripts).Excluding(y => y.CanDelete).Excluding(y => y.ModifiedOn));

            applicantSummary.ApplicationSummaries.Should().ContainSingle();
            var applicationSummary = applicantSummary.ApplicationSummaries.First();

            applicationSummary.ProgramChoices.Should().HaveCount(3);
            applicationSummary.TranscriptRequests.Should().BeNull();
            applicationSummary.ShoppingCartDetails.Should().BeNull();
            applicationSummary.Offers.Should().BeNull();
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateApplicantInfo_ShouldPass_OcasUser()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var applicant = new ApplicantUpdateInfo
            {
                FirstName = "SueA",
                LastName = "FourA",
                BirthDate = "1960-02-09"
            };
            var currentApplicant = await Client.UpdateApplicantInfo(TestConstants.Applicant.ApplicantUpdateInfoId, applicant);

            // Assert
            currentApplicant.Should().NotBeNull();
            currentApplicant.FirstName.Should().BeEquivalentTo(applicant.FirstName);
            currentApplicant.LastName.Should().BeEquivalentTo(applicant.LastName);
            currentApplicant.BirthDate.Should().BeEquivalentTo(applicant.BirthDate);
        }

        [Fact]
        [IntegrationTest]
        public async Task UpsertAcademicRecord_ShouldPass_When_Creating()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();
            var academicRecordBase = _modelFaker.GetAcademicRecordBase().Generate();
            academicRecordBase.ApplicantId = applicant.Id;

            // Act
            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);
            var result = await Client.UpsertAcademicRecord(applicant.Id, academicRecordBase);

            // Assert
            result.Should().BeOfType<UpsertAcademicRecordResult>().And.NotBeNull();
            result.AcademicRecord.Should().BeEquivalentTo(academicRecordBase, opt =>
                opt.Excluding(z => z.SchoolId)
                .ExcludingMissingMembers());
            result.AcademicRecord.ModifiedBy.Should().Be(TestConstants.Identity.Providers.OcasAdfs.QaBoUsername);
            result.AcademicRecord.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, 10.Seconds());
            result.SupportingDocument.Should().NotBeNull();
        }

        [Fact]
        [IntegrationTest]
        public async Task UpsertAcademicRecord_ShouldPass_When_Updating()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();

            var academicRecordBase = _modelFaker.GetAcademicRecordBase().Generate();
            academicRecordBase.ApplicantId = applicant.Id;
            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);
            var createdResult = await Client.UpsertAcademicRecord(applicant.Id, academicRecordBase);

            var academicRecordToUpdate = _modelFaker.GetAcademicRecordBase().Generate();
            academicRecordToUpdate.SchoolId = null;
            academicRecordToUpdate.ApplicantId = createdResult.AcademicRecord.ApplicantId;

            // Act
            var updatedResult = await Client.UpsertAcademicRecord(applicant.Id, academicRecordToUpdate);

            // Assert
            updatedResult.Should().BeOfType<UpsertAcademicRecordResult>().And.NotBeNull();
            updatedResult.AcademicRecord.Should().BeEquivalentTo(academicRecordToUpdate, opt =>
                opt.Excluding(z => z.SchoolId)
                .ExcludingMissingMembers());
            updatedResult.AcademicRecord.ModifiedBy.Should().Be(TestConstants.Identity.Providers.OcasAdfs.QaBoUsername);
            updatedResult.AcademicRecord.ModifiedOn.Should().BeOnOrAfter(createdResult.AcademicRecord.ModifiedOn);
            updatedResult.SupportingDocument.Should().BeNull();
        }

        [Fact]
        [IntegrationTest]
        public void UpsertAcademicRecord_ShouldThrow_When_AcademicRecord_Null()
        {
            // Arrange && Act
            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);
            Func<Task<UpsertAcademicRecordResult>> func = () => Client.UpsertAcademicRecord(Guid.NewGuid(), null);

            // Assert
            func.Should().Throw<StatusCodeException>()
                 .Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
