using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapApplicationCycle_ShouldPass()
        {
            // Arrange
            var appCycleStatus = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.ApplicationCycleStatuses);
            var startDate = _dataFakerFixture.Faker.Date.Past(2, DateTime.UtcNow);
            var considerationDate = TimeZoneInfo.ConvertTimeToUtc(new DateTime(startDate.Year, 2, 1, 23, 59, 59), Constants.TimeZone.Est);
            var dto = new Dto.ApplicationCycle
            {
                StatusId = appCycleStatus.Id,
                StartDate = startDate,
                EndDate = _dataFakerFixture.Faker.Date.Past(1, startDate)
            };

            // Act
            var applicationCycle = _apiMapper.MapApplicationCycle(dto, appCycleStatus, considerationDate);

            // Assert
            applicationCycle.Should().NotBeNull();
            applicationCycle.Status.Should().Be(appCycleStatus.Code);
        }
    }
}
