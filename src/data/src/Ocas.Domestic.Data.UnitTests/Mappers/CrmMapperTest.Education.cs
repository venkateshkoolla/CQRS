using FluentAssertions;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Data.TestFramework.Extensions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Data.UnitTests.Mappers
{
    public partial class CrmMapperTest
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapEducationBase_ShouldReturnCrmEntity_WhenInternationalEducation()
        {
            // Arrange
            var model = _dataFakerFixture.Models.EducationBase.Generate("default, Intl");

            // Act
            var entity = _mapper.MapEducationBase(model);

            // Assert
            entity.Should().BeOfType<ocaslr_education>().And.NotBeNull();
            entity.Id.Should().BeEmpty();
            entity.ocaslr_attendedfromdate.Should().Be(model.AttendedFrom.YearMonthToDateTime());
            entity.ocaslr_countryid.Id.Should().Be(model.CountryId.Value);
            entity.ocaslr_currentlyattending.Should().Be(model.CurrentlyAttending.Value);
            entity.ocaslr_attendedtodate.Should().Be(model.AttendedTo.YearMonthToDateTime());
            entity.ocaslr_firstnameonrecord.Should().Be(model.FirstNameOnRecord);
            entity.ocaslr_institutename.Should().Be(model.InstituteName);
            entity.ocaslr_institutetypeid.Id.Should().Be(model.InstituteTypeId.Value);
            entity.ocaslr_lastnameonrecord.Should().Be(model.LastNameOnRecord);
            entity.ocaslr_levelachievedid.Id.Should().Be(model.LevelAchievedId.Value);
            entity.ocaslr_major.Should().Be(model.Major);
            entity.ocaslr_oen.Should().Be(model.OntarioEducationNumber);
            entity.ocaslr_credentialreceivedother.Should().Be(model.OtherCredential);

            if (model.CredentialId.HasValue)
            {
                entity.ocaslr_credentialid.Id.Should().Be(model.CredentialId.Value);
            }
            else
            {
                entity.ocaslr_credentialid.Should().BeNull();
            }

            if (model.Graduated.HasValue)
            {
                entity.ocaslr_graduatestatus.Should().Be(model.Graduated.Value);
            }
            else
            {
                entity.ocaslr_graduatestatus.Should().BeNull();
            }
        }
    }
}
