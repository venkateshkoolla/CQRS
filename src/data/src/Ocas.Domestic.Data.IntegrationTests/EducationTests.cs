using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class EducationTests : BaseTest
    {
        [Fact]
        public async Task GetEducation_ShouldPass()
        {
            // Arrange & Act
            var education = await Context.GetEducation(TestConstants.Education.TestApplicant3Intl);

            // Assert
            education.Should().NotBe(null);
        }

        [Fact]
        public async Task GetEducations_ShouldPass()
        {
            // Arrange & Act
            var educations = await Context.GetEducations(TestConstants.OcasApplicants.TestApplicant3Id);

            // Assert
            educations.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateEducation_ShouldPass_WhenInternational()
        {
            Contact applicant = null;
            Education entity = null;
            try
            {
                // Arrange
                var applicantBase = DataFakerFixture.Models.ContactBase.Generate("default, Applicant");
                applicant = await Context.CreateContact(applicantBase);

                var model = DataFakerFixture.Models.EducationBase.Generate("default, Intl");
                model.ApplicantId = applicant.Id;

                // Act
                entity = await Context.CreateEducation(model);

                // Assert
                CheckEducationBaseFields(entity, model);
            }
            finally
            {
                // Cleanup
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (entity?.Id != null)
                    await Context.DeleteEducation(entity.Id);
            }
        }

        [Fact]
        public async Task DeleteEducation_ShouldPass()
        {
            // Arrange
            var model = DataFakerFixture.Models.EducationBase.Generate("default, Intl");

            // Act
            var entity = await Context.CreateEducation(model);
            await Context.DeleteEducation(entity.Id);
            var education = await Context.GetEducation(entity.Id);

            // Assert
            education.Should().BeNull();
        }

        [Fact]
        public async Task UpdateEducation_ShouldPass()
        {
            // Arrange
            var model = DataFakerFixture.Models.EducationBase.Generate("default, Intl");
            var before = await Context.GetEducation(TestConstants.Education.TestApplicant3Intl);
            model.ApplicantId = before.ApplicantId;
            before.AcademicUpgrade = model.AcademicUpgrade;
            before.AttendedFrom = model.AttendedFrom;
            before.AttendedTo = model.AttendedTo;
            before.CityId = model.CityId;
            before.CountryId = model.CountryId;
            before.CredentialId = model.CredentialId;
            before.CurrentlyAttending = model.CurrentlyAttending;
            before.FirstNameOnRecord = model.FirstNameOnRecord;
            before.Graduated = model.Graduated;
            before.InstituteId = model.InstituteId;
            before.InstituteName = model.InstituteName;
            before.InstituteTypeId = model.InstituteTypeId;
            before.LastGradeCompletedId = model.LastGradeCompletedId;
            before.LevelAchievedId = model.LevelAchievedId;
            before.LevelOfStudiesId = model.LevelOfStudiesId;
            before.Major = model.Major;
            before.OntarioEducationNumber = model.OntarioEducationNumber;
            before.OtherCredential = model.OtherCredential;
            before.ProvinceId = model.ProvinceId;
            before.StudentNumber = model.StudentNumber;
            before.ModifiedBy = model.ModifiedBy;

            // Act
            var after = await Context.UpdateEducation(before);

            // Assert
            after.Should().NotBeNull();
            CheckEducationFields(after, before);
        }

        private static void CheckEducationFields(Education entity, Education model)
        {
            entity.Id.Should().Be(model.Id);

            CheckEducationBaseFields(entity, model);
        }

        private static void CheckEducationBaseFields(Education entity, EducationBase model)
        {
            entity.Id.Should().NotBeEmpty();
            entity.AcademicUpgrade.Should().Be(model.AcademicUpgrade);
            entity.ApplicantId.Should().Be(model.ApplicantId);
            entity.AttendedFrom.Should().Be(model.AttendedFrom);
            entity.AttendedTo.Should().Be(model.AttendedTo);
            entity.CityId.Should().Be(model.CityId);
            entity.CountryId.Should().Be(model.CountryId);
            entity.CredentialId.Should().Be(model.CredentialId);
            entity.CurrentlyAttending.Should().Be(model.CurrentlyAttending);
            entity.FirstNameOnRecord.Should().Be(model.FirstNameOnRecord);
            entity.Graduated.Should().Be(model.Graduated);
            entity.InstituteId.Should().Be(model.InstituteId);
            entity.InstituteName.Should().Be(model.InstituteName);
            entity.InstituteTypeId.Should().Be(model.InstituteTypeId);
            entity.LastGradeCompletedId.Should().Be(model.LastGradeCompletedId);
            entity.LevelAchievedId.Should().Be(model.LevelAchievedId);
            entity.LevelOfStudiesId.Should().Be(model.LevelOfStudiesId);
            entity.Major.Should().Be(model.Major);
            entity.OntarioEducationNumber.Should().Be(model.OntarioEducationNumber);
            entity.OtherCredential.Should().Be(model.OtherCredential);
            entity.ProvinceId.Should().Be(model.ProvinceId);
            entity.StudentNumber.Should().Be(model.StudentNumber);

            entity.ModifiedBy.Should().Be(model.ModifiedBy);
        }
    }
}
