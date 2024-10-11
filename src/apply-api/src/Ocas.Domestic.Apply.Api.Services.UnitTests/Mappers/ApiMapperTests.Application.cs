using FluentAssertions;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapApplication_ShouldPass_When_NoCompletedSteps()
        {
            // Arrange
            var dto = new Dto.Application
            {
                CompletedSteps = null
            };

            // Act
            var application = _apiMapper.MapApplication(dto);

            // Assert
            application.ProgramsComplete.Should().BeFalse();
            application.TranscriptsComplete.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplication_ShouldPass_When_ProgramChoice()
        {
            // Arrange
            var dto = new Dto.Application
            {
                CompletedSteps = (int)ApplicationCompletedSteps.ProgramChoice
            };

            // Act
            var application = _apiMapper.MapApplication(dto);

            // Assert
            application.ProgramsComplete.Should().BeTrue();
            application.TranscriptsComplete.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapApplication_ShouldPass_When_TranscriptRequests()
        {
            // Arrange
            var dto = new Dto.Application
            {
                CompletedSteps = (int)ApplicationCompletedSteps.TranscriptRequests
            };

            // Act
            var application = _apiMapper.MapApplication(dto);

            // Assert
            application.ProgramsComplete.Should().BeTrue();
            application.TranscriptsComplete.Should().BeTrue();
        }
    }
}
