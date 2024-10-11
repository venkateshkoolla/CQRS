using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OntarioStudentCourseCreditTests : BaseTest
    {
        [Fact]
        public async Task GetOntarioStudentCourseCreditRecords_ShouldPass()
        {
            // Arrange
            var transcripts = await Context.GetTranscripts(new GetTranscriptOptions
            {
                ContactId = TestConstants.OntarioStudentCourseCredit.ApplicantId
            });
            var transcript = transcripts.First();

            var options = new GetOntarioStudentCourseCreditOptions
            {
                TranscriptId = transcript.Id
            };

            // Act
            var ontarioStudentCourseCreditList = await Context.GetOntarioStudentCourseCredits(options);

            // Assert
            ontarioStudentCourseCreditList.Should().NotBeNull();
            ontarioStudentCourseCreditList.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetOntarioStudentCourseCreditRecord_ShouldPass()
        {
            // Act
            var ontarioStudentCourseCreditData = await Context.GetOntarioStudentCourseCredit(TestConstants.OntarioStudentCourseCredit.Id);

            // Assert
            ontarioStudentCourseCreditData.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var applicantBase = DataFakerFixture.Models.ContactBase.Generate();
            var applicant = await Context.CreateContact(applicantBase);

            var model = DataFakerFixture.Models.OntarioStudentCourseCreditBase.Generate();
            model.TranscriptId = TestConstants.Transcripts.Id;
            model.ApplicantId = applicant.Id;

            // Act
            var entity = await Context.CreateOntarioStudentCourseCredit(model);

            // Assert
            CheckOntarioStudentCourseCreditBaseFields(entity, model);

            //Cleanup
            if (applicant?.Id != null)
                await Context.DeleteContact(applicant.Id);

            if (entity?.Id != null)
                await Context.DeleteOntarioStudentCourseCredit(entity.Id);
        }

        [Fact]
        public async Task DeleteOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var applicantBase = DataFakerFixture.Models.ContactBase.Generate();
            var applicant = await Context.CreateContact(applicantBase);

            var model = DataFakerFixture.Models.OntarioStudentCourseCreditBase.Generate();
            model.TranscriptId = TestConstants.Transcripts.Id;
            model.ApplicantId = applicant.Id;
            var entity = await Context.CreateOntarioStudentCourseCredit(model);

            // Act
            await Context.DeleteOntarioStudentCourseCredit(entity.Id);
            var ontarioStudentCourseCredit = await Context.GetOntarioStudentCourseCredit(entity.Id);

            // Assert
            ontarioStudentCourseCredit.Should().BeNull();
        }

        [Fact]
        public async Task UpdateOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var model = DataFakerFixture.Models.OntarioStudentCourseCreditBase.Generate();
            var ontarioStudentCourseCreditOptions = new GetOntarioStudentCourseCreditOptions
            {
                Id = TestConstants.OntarioStudentCourseCredit.Id
            };
            var beforeCollection = await Context.GetOntarioStudentCourseCredits(ontarioStudentCourseCreditOptions);
            var before = beforeCollection.FirstOrDefault();
            model.ApplicantId = before.ApplicantId;
            before.ModifiedBy = model.ModifiedBy;
            before.Credit = model.Credit;
            before.CourseCode = model.CourseCode;
            before.CompletedDate = model.CompletedDate;
            before.Notes = model.Notes;
            before.CourseMident = model.CourseMident;
            before.TranscriptId = model.TranscriptId;
            before.Grade = model.Grade;
            before.CourseStatusId = model.CourseStatusId;
            before.CourseTypeId = model.CourseTypeId;
            before.GradeTypeId = model.GradeTypeId;
            before.CourseDeliveryId = model.CourseDeliveryId;

            // Act
            var after = await Context.UpdateOntarioStudentCourseCredit(before);

            // Assert
            after.Should().NotBeNull();
            CheckOntarioStudentCourseCreditFields(after, before);
        }

        private static void CheckOntarioStudentCourseCreditFields(OntarioStudentCourseCredit entity, OntarioStudentCourseCredit model)
        {
            entity.Id.Should().Be(model.Id);

            CheckOntarioStudentCourseCreditBaseFields(entity, model);
        }

        private static void CheckOntarioStudentCourseCreditBaseFields(OntarioStudentCourseCredit entity, OntarioStudentCourseCreditBase model)
        {
            entity.Id.Should().NotBeEmpty();
            entity.ApplicantId.Should().Be(model.ApplicantId);
            entity.ModifiedBy.Should().Be(model.ModifiedBy);
            var dblModelCredit = (double)model.Credit;
            var dblEntityCredit = (double)entity.Credit;
            dblEntityCredit.Should().BeApproximately(dblModelCredit, .1);
            entity.CourseCode.Should().Be(model.CourseCode);
            entity.CompletedDate.Should().Be(model.CompletedDate);
            entity.Notes.Should().Be(model.Notes);
            entity.CourseMident.Should().Be(model.CourseMident);
            entity.TranscriptId.Should().Be(model.TranscriptId);
            entity.Grade.Should().Be(model.Grade);
            entity.CourseStatusId.Should().Be(model.CourseStatusId);
            entity.CourseTypeId.Should().Be(model.CourseTypeId);
            entity.GradeTypeId.Should().Be(model.GradeTypeId);
            entity.CourseDeliveryId.Should().Be(model.CourseDeliveryId);
        }
    }
}
