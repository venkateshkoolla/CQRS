using System.Collections.Generic;
using System.Linq;
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
        public void MapApplicantMessage_ShouldPass()
        {
            // Arrange
            var dto = new Dto.ApplicantMessage
            {
                HasRead = true,
                LocalizedSubject = _dataFakerFixture.Faker.Name.JobTitle(),
                LocalizedText = _dataFakerFixture.Faker.Lorem.Sentence(15)
            };
            var dtos = new List<Dto.ApplicantMessage> { dto };

            // Act
            var applicantMessages = _apiMapper.MapApplicantMessages(dtos);

            // Assert
            applicantMessages.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var applicantMessage = applicantMessages.First();
            applicantMessage.Read.Should().BeTrue();
            applicantMessage.Title.Should().Be(dto.LocalizedSubject);
            applicantMessage.Message.Should().Be(dto.LocalizedText);
        }
    }
}
