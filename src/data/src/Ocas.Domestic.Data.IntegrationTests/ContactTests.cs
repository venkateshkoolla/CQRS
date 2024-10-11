using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ContactTests : BaseTest
    {
        [Fact]
        public async Task GetApplicantSummary_ShouldPass()
        {
            // Arrange & Act
            var summary = await Context.GetApplicantSummary(new GetApplicantSummaryOptions
            {
                ApplicantId = TestConstants.ApplicantSummary.ApplicantId,
                Locale = Locale.English
            });

            // Assert
            summary.Should().NotBeNull();
        }

        [Fact]
        public async Task GetContactByUsername_ShouldPass()
        {
            // Act
            var contact = await Context.GetContact(TestConstants.OcasApplicants.TestApplicant1Username);

            // Assert
            contact.Should().NotBe(null);
        }

        [Fact]
        public async Task GetContact_ShouldPass()
        {
            // Arrange
            var contactByUsername = await Context.GetContact(TestConstants.OcasApplicants.TestApplicant1Username);

            // Act
            var contact = await Context.GetContact(contactByUsername.Id);

            // Assert
            contact.Should().NotBe(null);
        }

        [Fact]
        public async Task GetContactSubjectId_ShouldPass()
        {
            // Arrange
            var contact = await Context.GetContact(TestConstants.OcasApplicants.TestApplicant1Username);

            // Act
            var contactSubjectId = await Context.GetContactSubjectId(contact.Id);

            // Assert
            contactSubjectId.Should().BeOneOf(contact.Username, contact.SubjectId);
        }

        [Fact]
        public async Task CanAccessApplicantGet_CollegeUser_ShouldPass()
        {
            var result = await Context.CanAccessApplicant(TestConstants.CollegeUser.ApplicantId, TestConstants.CollegeUser.PartnerId, UserType.CollegeUser);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanAccessApplicantGet_HighSchoolUser_ShouldPass()
        {
            var result = await Context.CanAccessApplicant(TestConstants.HighSchoolUser.ApplicantId, TestConstants.HighSchoolUser.PartnerId, UserType.HighSchoolUser);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanAccessApplicantGet_HighSchoolBoardUser_ShouldPass()
        {
            var result = await Context.CanAccessApplicant(TestConstants.HighSchoolUser.ApplicantId, TestConstants.HighSchoolUser.BoardId, UserType.HighSchoolBoardUser);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CanAccessApplicantGet_UnknownUser_ShouldPass()
        {
            var result = await Context.CanAccessApplicant(TestConstants.HighSchoolUser.ApplicantId, TestConstants.HighSchoolUser.PartnerId, 0);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsActive_ShouldPass()
        {
            Contact entity = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate("default,Applicant");
                entity = await Context.CreateContact(model);

                var entityActive = await Context.IsActive(entity.Id);

                entity.AccountStatusId = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AccountStatuses.Where(x => x.Code != TestConstants.Contact.AccountStatuses.Active)).Id;
                await Context.UpdateContact(entity);
                var entityDead = await Context.IsActive(entity.Id);

                entityActive.Should().BeTrue();
                entityDead.Should().BeFalse();
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteContact(entity.Id);
            }
        }

        [Fact]
        public async Task IsDuplicateContact_ShouldPass()
        {
            Contact entity = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate("default,Applicant");
                entity = await Context.CreateContact(model);

                var checkSelf = await Context.IsDuplicateContact(entity.Id, entity.FirstName, entity.LastName, entity.BirthDate);
                checkSelf.Should().BeFalse();

                var checkOther = await Context.IsDuplicateContact(Guid.NewGuid(), entity.FirstName, entity.LastName, entity.BirthDate);
                checkOther.Should().BeTrue();
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteContact(entity.Id);
            }
        }

        [Fact]
        public async Task IsDuplicateContactByEmail_ShouldPass()
        {
            Contact entity = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate("default,Applicant");
                entity = await Context.CreateContact(model);

                var checkSelf = await Context.IsDuplicateContact(entity.Id, entity.Email);
                checkSelf.Should().BeFalse();

                var checkOther = await Context.IsDuplicateContact(Guid.NewGuid(), entity.Email);
                checkOther.Should().BeTrue();
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteContact(entity.Id);
            }
        }

        [Fact]
        public async Task IsDuplicateOen_ShouldPass_WhenOenNullOrDefault()
        {
            Contact entity = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate();
                entity = await Context.CreateContact(model);
                var entityAfter = await Context.GetContact(entity.Id);

                //Act
                var checkNull = await Context.IsDuplicateOen(entityAfter.Id, entityAfter.OntarioEducationNumber);
                var checkDefault = await Context.IsDuplicateOen(entityAfter.Id, TestConstants.Contact.OntarioEducationNumberDefault);

                // Assert
                checkNull.Should().BeFalse();
                checkDefault.Should().BeFalse();
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteContact(entity.Id);
            }
        }

        [Fact]
        public async Task CreateContact_ShouldPass()
        {
            Contact entity = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate();

                // Act
                entity = await Context.CreateContact(model);

                // Assert
                CheckContactBaseFields(entity, model);
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteContact(entity.Id);
            }
        }

        [Fact]
        public async Task UpdateAcceptedPrivacyStatement_ShouldPass()
        {
            Contact entityBefore = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate();
                entityBefore = await Context.CreateContact(model);

                // Act
                var entityUpdates = DataFakerFixture.Models.Contact.Generate();
                entityBefore.MiddleName = entityUpdates.MiddleName;
                entityBefore.PreviousLastName = entityUpdates.PreviousLastName;
                entityBefore.GenderId = entityUpdates.GenderId;
                entityBefore.FirstGenerationId = entityUpdates.FirstGenerationId;
                entityBefore.FirstLanguageId = entityUpdates.FirstLanguageId;
                entityBefore.TitleId = entityUpdates.TitleId;

                entityBefore.AcceptedPrivacyStatementId =
                    entityBefore.AcceptedPrivacyStatementId.HasValue
                        ? null
                        : DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PrivacyStatements).Id as Guid?;
                var entityAfter = await Context.UpdateContact(entityBefore);

                // Assert
                CheckContactFields(entityAfter, entityBefore);
                (entityAfter.AcceptedPrivacyStatementId == model.AcceptedPrivacyStatementId).Should().BeFalse();
            }
            finally
            {
                // Cleanup
                if (entityBefore?.Id != null)
                    await Context.DeleteContact(entityBefore.Id);
            }
        }

        [Fact]
        public async Task UpdateContact_ShouldPass()
        {
            Contact entityBefore = null;
            try
            {
                // Arrange
                entityBefore = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());
                var model = DataFakerFixture.Models.Contact.Generate();
                model.Id = entityBefore.Id;
                model.AccountNumber = entityBefore.AccountNumber;
                model.SubjectId = entityBefore.SubjectId;

                // Act
                var entityAfter = await Context.UpdateContact(model);

                // Assert
                CheckContactFields(entityAfter, model);
            }
            finally
            {
                // Cleanup
                if (entityBefore?.Id != null)
                    await Context.DeleteContact(entityBefore.Id);
            }
        }

        [Fact]
        public async Task GetCompletedStep_ShouldPass()
        {
            Contact entity = null;
            try
            {
                // Arrange
                entity = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate());
                var model = DataFakerFixture.Models.Contact.Generate();
                model.Id = entity.Id;
                model.AccountNumber = entity.AccountNumber;
                model.SubjectId = entity.SubjectId;
                await Context.UpdateContact(model);

                // Act
                var completedStep = await Context.GetCompletedStep(entity.Id);

                // Assert
                completedStep.Should().Be(CompletedSteps.CitizenshipandResidency);
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteContact(entity.Id);
            }
        }

        [Fact]
        public async Task UpdateCompletedSteps_ShouldPass()
        {
            Contact entity = null;
            try
            {
                // Arrange
                var model = DataFakerFixture.Models.ContactBase.Generate();
                entity = await Context.CreateContact(model);

                // Act
                var completedSteps = await Context.UpdateCompletedSteps(entity.Id);

                // Assert
                completedSteps.Should().Be(null);
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteContact(entity.Id);
            }
        }

        [Fact]
        public async Task DeleteContact_ShouldPass()
        {
            // Arrange
            var model = DataFakerFixture.Models.ContactBase.Generate();

            // Act
            var entity = await Context.CreateContact(model);
            await Context.DeleteContact(entity.Id);
            var contactById = await Context.GetContact(entity.Id);
            var contactByUsername = await Context.GetContact(entity.Username);

            // Assert
            contactById.Should().BeNull();
            contactByUsername.Should().BeNull();
        }

        private static void CheckContactBaseFields(Contact entity, ContactBase model)
        {
            entity.Id.Should().NotBeEmpty();
            entity.FirstName.Should().Be(model.FirstName);
            entity.LastName.Should().Be(model.LastName);
            entity.PreferredName.Should().Be(model.PreferredName);
            entity.Username.Should().Be(model.Username);
            entity.SubjectId.Should().Be(model.SubjectId);
            entity.Email.Should().Be(model.Email);
            entity.BirthDate.Should().Be(model.BirthDate);
            entity.SourceId.Should().Be(model.SourceId);
            entity.AccountStatusId.Should().Be(model.AccountStatusId);
            entity.ContactType.Should().Be((int)model.ContactType);
            entity.AcceptedPrivacyStatementId.Should().Be(model.AcceptedPrivacyStatementId);
            entity.PreferredLanguageId.Should().Be(model.PreferredLanguageId);
            entity.SourcePartnerId.Should().Be(model.SourcePartnerId);
        }

        private void CheckContactFields(Contact entity, Contact model)
        {
            CheckContactBaseFields(entity, model);

            entity.Id.Should().Be(model.Id);
            entity.AccountNumber.Should().Be(model.AccountNumber);
            entity.DoNotSendMM.Should().Be(model.DoNotSendMM);
            entity.LastUsedInCampaign.Should().Be(model.LastUsedInCampaign);
            entity.MiddleName.Should().Be(model.MiddleName);
            entity.GenderId.Should().Be(model.GenderId);
            entity.FirstGenerationId.Should().Be(model.FirstGenerationId);
            entity.FirstLanguageId.Should().Be(model.FirstLanguageId);
            entity.TitleId.Should().Be(model.TitleId);
            entity.PreferredCorrespondenceMethodId.Should().Be(model.PreferredCorrespondenceMethodId);
            entity.CountryOfBirthId.Should().Be(model.CountryOfBirthId);
            entity.CountryOfCitizenshipId.Should().Be(model.CountryOfCitizenshipId);
            entity.DateOfArrival.Should().Be(model.DateOfArrival);
            entity.StatusInCanadaId.Should().Be(model.StatusInCanadaId);
            entity.StatusOfVisaId.Should().Be(model.StatusOfVisaId);
            entity.HighSchoolEnrolled.Should().Be(model.HighSchoolEnrolled);
            entity.HighSchoolGraduated.Should().Be(model.HighSchoolGraduated);
            entity.HighSchoolGraduationDate.Should().Be(model.HighSchoolGraduationDate);
            entity.OntarioEducationNumber.Should().Be(model.OntarioEducationNumber);
            entity.OntarioEducationNumberLock.Should().NotBeTrue();

            if (model.MailingAddress != null)
            {
                entity.MailingAddress.Should().NotBeNull();
                entity.MailingAddress.CountryId.Should().Be(model.MailingAddress.CountryId);
                entity.MailingAddress.Country.Should().Be(model.MailingAddress.Country);
                entity.MailingAddress.City.Should().Be(model.MailingAddress.City);
                entity.MailingAddress.Street.Should().Be(model.MailingAddress.Street);
                entity.MailingAddress.PostalCode.Should().Be(model.MailingAddress.PostalCode);
                entity.MailingAddress.Verified.Should().Be(model.MailingAddress.Verified);
                var provinceState = DataFakerFixture.SeedData.ProvinceStates.FirstOrDefault(x => x.Code == model.MailingAddress.ProvinceState);
                entity.MailingAddress.ProvinceStateId.Should().Be(provinceState?.Id);
                entity.MailingAddress.ProvinceState.Should().Be(model.MailingAddress.ProvinceState);
            }
        }
    }
}
