using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Models;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class OntarioStudentCourseCreditsControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _modelFakerFixture;

        public OntarioStudentCourseCreditsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task CreateOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);
            var applicant = await ApplyClientFixture.CreateNewApplicant();

            var academicRecord = _modelFakerFixture.GetAcademicRecordBase().Generate();
            academicRecord.ApplicantId = applicant.Id;
            await Client.UpsertAcademicRecord(applicant.Id, academicRecord);

            var ontarioStudentCourseCreditBase = _modelFakerFixture.GetOntarioStudentCourseCreditBase().Generate();
            ontarioStudentCourseCreditBase.ApplicantId = applicant.Id;

            OntarioStudentCourseCredit ontarioStudentCourseCredit = null;
            try
            {
                // Act
                ontarioStudentCourseCredit = await Client.PostOntarioStudentCourseCredit(ontarioStudentCourseCreditBase);

                // Assert
                ontarioStudentCourseCredit.Should().NotBeNull();
                ontarioStudentCourseCredit.Id.Should().NotBeEmpty();
                ontarioStudentCourseCredit.Should().BeEquivalentTo(ontarioStudentCourseCreditBase, opts => opts
                .Excluding(x => x.Credit)
                .Excluding(x => x.CompletedDate)
                .Excluding(x => x.SupplierMident));
                ontarioStudentCourseCredit.ModifiedBy.Should().Be(TestConstants.Identity.Providers.OcasAdfs.QaBoUsername);
            }
            finally
            {
                //Cleanup from DB
                if (ontarioStudentCourseCredit != null)
                    await Client.DeleteOntarioStudentCourseCredit(ontarioStudentCourseCredit.Id);
            }
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();

            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);

            var academicRecord = _modelFakerFixture.GetAcademicRecordBase().Generate();
            academicRecord.ApplicantId = applicant.Id;
            await Client.UpsertAcademicRecord(applicant.Id, academicRecord);

            var ontarioStudentCourseCreditBase = _modelFakerFixture.GetOntarioStudentCourseCreditBase().Generate();
            ontarioStudentCourseCreditBase.ApplicantId = applicant.Id;

            OntarioStudentCourseCredit before = null;
            try
            {
                before = await Client.PostOntarioStudentCourseCredit(ontarioStudentCourseCreditBase);
                // Make changes
                var changeModel = _modelFakerFixture.GetOntarioStudentCourseCredit().Generate();
                changeModel.Id = before.Id;
                changeModel.ApplicantId = before.ApplicantId;
                changeModel.TranscriptId = before.TranscriptId;

                // Act
                var after = await Client.UpdateOntarioStudentCourseCredit(changeModel);

                // Assert
                after.Should().NotBeNull();
                after.Id.Should().Be(before.Id);
                after.Should().BeEquivalentTo(changeModel, opts => opts
                .Excluding(x => x.ModifiedOn)
                .Excluding(x => x.ModifiedBy)
                .Excluding(x => x.CompletedDate)
                .Excluding(x => x.SupplierMident));
            }
            finally
            {
                //Cleanup from DB
                if (before != null)
                    await Client.DeleteOntarioStudentCourseCredit(before.Id);
            }
        }

        [Fact]
        [IntegrationTest]
        public async Task DeleteOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var applicant = await ApplyClientFixture.CreateNewApplicant();

            Client.WithAccessToken(_identityUserFixture.OcasBoUser.AccessToken);

            var academicRecord = _modelFakerFixture.GetAcademicRecordBase().Generate();
            academicRecord.ApplicantId = applicant.Id;
            await Client.UpsertAcademicRecord(applicant.Id, academicRecord);

            var ontarioStudentCourseCreditBase = _modelFakerFixture.GetOntarioStudentCourseCreditBase().Generate();
            ontarioStudentCourseCreditBase.ApplicantId = applicant.Id;
            var ontarioStudentCourseCredit = await Client.PostOntarioStudentCourseCredit(ontarioStudentCourseCreditBase);

            // Act
            await Client.DeleteOntarioStudentCourseCredit(ontarioStudentCourseCredit.Id);

            // Assert
            var applicantSummary = await Client.GetApplicantSummary(applicant.Id);
            applicantSummary.OntarioStudentCourseCredits.Should().BeEmpty();
        }
    }
}