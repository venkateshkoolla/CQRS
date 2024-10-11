using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class AcademicRecordTests : BaseTest
    {
        [Fact]
        public async Task GetAcademicRecords_ShouldPass()
        {
            // Act
            var academicDataList = await Context.GetAcademicRecords(TestConstants.AcademicRecord.ApplicantId);

            // Assert
            academicDataList.Should().NotBeNull();
            academicDataList.Should().ContainSingle();
        }

        [Fact]
        public async Task GetAcademicRecord_ShouldPass()
        {
            // Act
            var academicData = await Context.GetAcademicRecord(TestConstants.AcademicRecord.Id);

            // Assert
            academicData.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAcademicRecord_ShouldPass()
        {
            Contact applicant = null;
            AcademicRecord entity = null;
            try
            {
                // Arrange
                var applicantBase = DataFakerFixture.Models.ContactBase.Generate();
                applicant = await Context.CreateContact(applicantBase);

                var model = DataFakerFixture.Models.AcademicRecordBase.Generate();
                model.ApplicantId = applicant.Id;
                model.Name = applicant.AccountNumber;

                // Act
                entity = await Context.CreateAcademicRecord(model);

                // Assert
                CheckAcademicRecordBaseFields(entity, model);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (entity?.Id != null)
                    await Context.DeleteAcademicRecord(entity.Id);
            }
        }

        [Fact]
        public async Task DeleteAcademicRecord_ShouldPass()
        {
            // Arrange
            var applicantBase = DataFakerFixture.Models.ContactBase.Generate();
            var applicant = await Context.CreateContact(applicantBase);

            var model = DataFakerFixture.Models.AcademicRecordBase.Generate();
            model.ApplicantId = applicant.Id;
            model.Name = applicant.AccountNumber;
            var entity = await Context.CreateAcademicRecord(model);

            // Act
            await Context.DeleteAcademicRecord(entity.Id);
            var academicRecord = await Context.GetAcademicRecord(entity.Id);

            // Assert
            academicRecord.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAcademicRecord_ShouldPass()
        {
            // Arrange
            var model = DataFakerFixture.Models.AcademicRecordBase.Generate();
            var before = await Context.GetAcademicRecord(TestConstants.AcademicRecord.Id);
            model.ApplicantId = before.ApplicantId;
            before.CommunityInvolvementId = model.CommunityInvolvementId;
            before.HighestEducationId = model.HighestEducationId;
            before.HighSkillsMajorId = model.HighSkillsMajorId;
            before.LiteracyTestId = model.LiteracyTestId;
            before.ShsmCompletionId = model.ShsmCompletionId;
            before.StudentId = model.StudentId;
            before.Name = model.Name;
            before.DateCredentialAchieved = model.DateCredentialAchieved;
            before.ModifiedBy = model.ModifiedBy;

            // Act
            var after = await Context.UpdateAcademicRecord(before);

            // Assert
            after.Should().NotBeNull();
            CheckAcademicRecordFields(after, before);
        }

        private static void CheckAcademicRecordFields(AcademicRecord entity, AcademicRecord model)
        {
            entity.Id.Should().Be(model.Id);

            CheckAcademicRecordBaseFields(entity, model);
        }

        private static void CheckAcademicRecordBaseFields(AcademicRecord entity, AcademicRecordBase model)
        {
            entity.Id.Should().NotBeEmpty();
            entity.ApplicantId.Should().Be(model.ApplicantId);
            entity.CommunityInvolvementId.Should().Be(model.CommunityInvolvementId);
            entity.HighestEducationId.Should().Be(model.HighestEducationId);
            entity.HighSkillsMajorId.Should().Be(model.HighSkillsMajorId);
            entity.LiteracyTestId.Should().Be(model.LiteracyTestId);
            entity.ShsmCompletionId.Should().Be(model.ShsmCompletionId);
            entity.StudentId.Should().Be(model.StudentId);
            entity.Name.Should().Be(model.Name);
            entity.ModifiedBy.Should().Be(model.ModifiedBy);
        }
    }
}
