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
        public void MapCollegeInformations_ShouldPass()
        {
            // Arrange
            var dtoCollegeInfo = new Dto.CollegeInformation
            {
                LocalizedWelcomeText = _dataFakerFixture.Faker.Lorem.Sentence(5)
            };
            var dtoCollegeInfos = new List<Dto.CollegeInformation> { dtoCollegeInfo };

            // Act
            var collegeInfos = _apiMapper.MapCollegeInformations(dtoCollegeInfos);

            // Assert
            collegeInfos.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var collegeInfo = collegeInfos.First();
            collegeInfo.WelcomeText.Should().Be(dtoCollegeInfo.LocalizedWelcomeText);
        }
    }
}
