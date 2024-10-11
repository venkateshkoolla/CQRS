using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using DtoEnum = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Common.Services.UnitTests.Mappers
{
    public partial class ApiMapperBaseTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapProgramChoices_ShouldPass_When_SupplementalFeePaid()
        {
            // Arrange
            var dtoChoice = _mapper.Map<Dto.ProgramChoice>(_models.GetProgramChoice().Generate());
            dtoChoice.SupplementalFeePaid = false;

            var dtoShoppingCartDetail = new Dto.ShoppingCartDetail
            {
                ReferenceId = dtoChoice.CollegeId,
                Description = "This is a test description"
            };

            // Act
            var results = _apiMapper.MapProgramChoices(new List<Dto.ProgramChoice> { dtoChoice }, new List<Dto.ProgramIntake>(), _models.AllApplyLookups.ProgramIntakeAvailabilities, new List<Dto.ShoppingCartDetail> { dtoShoppingCartDetail });

            // Assert
            results.Should().ContainSingle();

            var choice = results.First();
            choice.SupplementalFeeDescription.Should().Be(dtoShoppingCartDetail.Description);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapProgramChoices_ShouldPass_When_IntakeClosed()
        {
            // Arrange
            var availability = _models.AllApplyLookups.ProgramIntakeAvailabilities.First(a => a.Code == Constants.ProgramIntakeAvailabilities.Closed);

            var dtoIntake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                AvailabilityId = availability.Id,
                EntryLevels = new List<Guid>(),
                State = DtoEnum.State.Active
            };

            var dtoChoice = _mapper.Map<Dto.ProgramChoice>(_models.GetProgramChoice().Generate());
            dtoChoice.ProgramIntakeId = dtoIntake.Id;

            // Act
            var results = _apiMapper.MapProgramChoices(new List<Dto.ProgramChoice> { dtoChoice }, new List<Dto.ProgramIntake> { dtoIntake }, _models.AllApplyLookups.ProgramIntakeAvailabilities, new List<Dto.ShoppingCartDetail>());

            // Assert
            results.Should().ContainSingle();

            var choice = results.First();
            choice.IsActive.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapProgramChoices_ShouldPass_When_IntakeInactive()
        {
            // Arrange
            var availability = _models.AllApplyLookups.ProgramIntakeAvailabilities.First(a => a.Code == Constants.ProgramIntakeAvailabilities.Open);

            var dtoIntake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                AvailabilityId = availability.Id,
                EntryLevels = new List<Guid>(),
                State = DtoEnum.State.Inactive
            };

            var dtoChoice = _mapper.Map<Dto.ProgramChoice>(_models.GetProgramChoice().Generate());
            dtoChoice.ProgramIntakeId = dtoIntake.Id;

            // Act
            var results = _apiMapper.MapProgramChoices(new List<Dto.ProgramChoice> { dtoChoice }, new List<Dto.ProgramIntake> { dtoIntake }, _models.AllApplyLookups.ProgramIntakeAvailabilities, new List<Dto.ShoppingCartDetail>());

            // Assert
            results.Should().ContainSingle();

            var choice = results.First();
            choice.IsActive.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapProgramChoices_ShouldPass_When_IntakeStartDate_Past_Months()
        {
            // Arrange
            var availability = _models.AllApplyLookups.ProgramIntakeAvailabilities.First(a => a.Code == Constants.ProgramIntakeAvailabilities.Open);

            var dtoIntake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                AvailabilityId = availability.Id,
                EntryLevels = new List<Guid>(),
                State = DtoEnum.State.Active,
                StartDate = DateTime.UtcNow.AddMonths(-Constants.ProgramChoices.MonthsToInactivity).ToStringOrDefault(Constants.DateFormat.IntakeStartDate)
            };

            var dtoChoice = _mapper.Map<Dto.ProgramChoice>(_models.GetProgramChoice().Generate());
            dtoChoice.ProgramIntakeId = dtoIntake.Id;

            // Act
            var results = _apiMapper.MapProgramChoices(new List<Dto.ProgramChoice> { dtoChoice }, new List<Dto.ProgramIntake> { dtoIntake }, _models.AllApplyLookups.ProgramIntakeAvailabilities, new List<Dto.ShoppingCartDetail>());

            // Assert
            results.Should().ContainSingle();

            var choice = results.First();
            choice.IsActive.Should().BeFalse();
        }
    }
}
