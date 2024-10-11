using System;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class GetCollegeApplicationCyclesValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly GetCollegeApplicationCyclesValidator _validator;

        public GetCollegeApplicationCyclesValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new GetCollegeApplicationCyclesValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetCollegeApplicationCyclesValidator_ShouldPass()
        {
            // Arrange
            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var model = new GetCollegeApplicationCycles
            {
                CollegeId = _dataFakerFixture.Faker.PickRandom(colleges).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetCollegeApplicationCyclesValidator_ShouldFail_When_CollegeId_Empty()
        {
            // Arrange
            var model = new GetCollegeApplicationCycles
            {
                CollegeId = Guid.Empty,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'College Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetCollegeApplicationCyclesValidator_ShouldFail_When_CollegeId_NotFound()
        {
            // Arrange
            var model = new GetCollegeApplicationCycles
            {
                CollegeId = Guid.NewGuid(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'College Id' does not exist: {model.CollegeId}");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetCollegeApplicationCyclesValidator_ShouldFail_When_User_Empty()
        {
            // Arrange
            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var model = new GetCollegeApplicationCycles
            {
                CollegeId = _dataFakerFixture.Faker.PickRandom(colleges).Id,
                User = null
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'User' must not be empty.");
        }
    }
}
