using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapProgramIntakeBriefs_ShouldPass_When_PropsEmpty()
        {
            // Arrange
            var props = string.Empty;
            var dbDtos = GenerateDtoProgramIntakes(out var dtoIntake);

            // Act
            var results = _apiMapper.MapProgramIntakeBriefs(
                dbDtos,
                _models.AllAdminLookups.StudyMethods,
                _models.AllAdminLookups.Colleges,
                _models.AllAdminLookups.Campuses,
                _models.AllAdminLookups.IntakeStatuses,
                _models.AllAdminLookups.IntakeAvailabilities,
                props);

            // Assert
            results.Should().ContainSingle();
            var intake = results.First();

            intake.Should().NotBeNull();
            intake.Id.Should().Be(dtoIntake.Id);
            intake.ProgramId.Should().NotBeNull().And.Be(dtoIntake.ProgramId);
            intake.ProgramCode.Should().NotBeNullOrEmpty().And.Be(dtoIntake.ProgramCode);
            intake.ProgramTitle.Should().NotBeNullOrEmpty().And.Be(dtoIntake.ProgramTitle);
            intake.DeliveryId.Should().NotBeNull().And.Be(dtoIntake.ProgramDeliveryId);
            intake.CollegeId.Should().NotBeNull().And.Be(dtoIntake.CollegeId);
            intake.CollegeName.Should().NotBeNullOrEmpty().And.Be(_models.AllAdminLookups.Colleges.First(c => c.Id == dtoIntake.CollegeId).Name);
            intake.CampusId.Should().NotBeNull().And.Be(dtoIntake.CampusId);
            intake.CampusName.Should().NotBeNullOrEmpty().And.Be(_models.AllAdminLookups.Campuses.First(c => c.Id == dtoIntake.CampusId).Name);
            intake.StartDate.Should().NotBeNullOrEmpty().And.Be(dtoIntake.StartDate);
            intake.IntakeStatusId.Should().NotBeNull().And.Be(dtoIntake.ProgramIntakeStatusId);
            intake.IntakeAvailabilityId.Should().NotBeNull().And.Be(dtoIntake.AvailabilityId);
            intake.EligibleEntryLevelIds.Should().NotBeNull().And.BeEquivalentTo(dtoIntake.EntryLevels);
            intake.Delivery.Should().NotBeEmpty().And.Be(_models.AllAdminLookups.StudyMethods.First(s => s.Id == dtoIntake.ProgramDeliveryId).Label);
            intake.IntakeStatus.Should().NotBeEmpty().And.Be(_models.AllAdminLookups.IntakeStatuses.First(s => s.Id == dtoIntake.ProgramIntakeStatusId).Label);
            intake.IntakeAvailability.Should().NotBeEmpty().And.Be(_models.AllAdminLookups.IntakeAvailabilities.First(a => a.Id == dtoIntake.AvailabilityId).Label);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapProgramIntakeBriefs_ShouldPass_When_Props()
        {
            // Arrange
            var props = string.Join(" ", new string[] { nameof(IntakeBrief.ProgramId), nameof(IntakeBrief.ProgramTitle), nameof(IntakeBrief.EligibleEntryLevelIds) });
            var dbDtos = GenerateDtoProgramIntakes(out var dtoIntake);

            // Act
            var results = _apiMapper.MapProgramIntakeBriefs(
                dbDtos,
                _models.AllAdminLookups.StudyMethods,
                _models.AllAdminLookups.Colleges,
                _models.AllAdminLookups.Campuses,
                _models.AllAdminLookups.IntakeStatuses,
                _models.AllAdminLookups.IntakeAvailabilities,
                props);

            // Assert
            results.Should().ContainSingle();
            var intake = results.First();

            intake.Should().NotBeNull();
            intake.Id.Should().Be(dtoIntake.Id);
            intake.ProgramId.Should().NotBeNull().And.Be(dtoIntake.ProgramId);
            intake.ProgramTitle.Should().NotBeNullOrEmpty().And.Be(dtoIntake.ProgramTitle);
            intake.EligibleEntryLevelIds.Should().NotBeNull().And.BeEquivalentTo(dtoIntake.EntryLevels);
            intake.ProgramCode.Should().BeNull();
            intake.DeliveryId.Should().BeNull();
            intake.CollegeId.Should().BeNull();
            intake.CollegeName.Should().BeNull();
            intake.CampusId.Should().BeNull();
            intake.CampusName.Should().BeNull();
            intake.StartDate.Should().BeNull();
            intake.IntakeStatusId.Should().BeNull();
            intake.IntakeAvailabilityId.Should().BeNull();

            intake.Delivery.Should().NotBeEmpty().And.Be(_models.AllAdminLookups.StudyMethods.First(s => s.Id == dtoIntake.ProgramDeliveryId).Label);
            intake.IntakeStatus.Should().NotBeEmpty().And.Be(_models.AllAdminLookups.IntakeStatuses.First(s => s.Id == dtoIntake.ProgramIntakeStatusId).Label);
            intake.IntakeAvailability.Should().NotBeEmpty().And.Be(_models.AllAdminLookups.IntakeAvailabilities.First(a => a.Id == dtoIntake.AvailabilityId).Label);
        }

        private IList<Dto.ProgramIntake> GenerateDtoProgramIntakes(out Dto.ProgramIntake dtoIntake)
        {
            dtoIntake = new Faker<Dto.ProgramIntake>()
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.ProgramId, _ => Guid.NewGuid())
                .RuleFor(x => x.ProgramCode, f => f.Random.AlphaNumeric(6))
                .RuleFor(x => x.ProgramTitle, f => f.Random.Word())
                .RuleFor(x => x.ProgramDeliveryId, f => f.PickRandom(_models.AllAdminLookups.StudyMethods).Id)
                .RuleFor(x => x.CollegeId, f => f.PickRandom(_models.AllAdminLookups.Colleges).Id)
                .RuleFor(x => x.CampusId, f => f.PickRandom(_models.AllAdminLookups.Campuses).Id)
                .RuleFor(x => x.StartDate, f => f.Date.Past().AsUtc().ToStringOrDefault(Constants.DateFormat.IntakeStartDate))
                .RuleFor(x => x.ProgramIntakeStatusId, f => f.PickRandom(_models.AllAdminLookups.IntakeStatuses).Id)
                .RuleFor(x => x.AvailabilityId, f => f.PickRandom(_models.AllAdminLookups.IntakeAvailabilities).Id)
                .RuleFor(x => x.EntryLevels, _ => _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.EntryLevels, 5).Select(e => e.Id).ToList())
                .Generate();
            return new List<Dto.ProgramIntake> { dtoIntake } as IList<Dto.ProgramIntake>;
        }
    }
}
