using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class GetSpecialCodeValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly Faker _faker;
        private readonly GetSpecialCodeValidator _validator;

        public GetSpecialCodeValidatorTests()
        {
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _validator = new GetSpecialCodeValidator(XunitInjectionCollection.LookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodeValidator_ShouldPass()
        {
            // Arrange
            var message = new GetSpecialCode
            {
                CollegeApplicationCycleId = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles).Id,
                SpecialCode = _faker.Random.String2(2),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodeValidator_ShouldFail_When_CollegeApplicationCycleId_Empty()
        {
            // Arrange
            var message = new GetSpecialCode
            {
                CollegeApplicationCycleId = Guid.Empty,
                SpecialCode = _faker.Random.String2(2),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'College Application Cycle Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodeValidator_ShouldFail_When_CollegeApplicationCycleId_NotFound()
        {
            // Arrange
            var message = new GetSpecialCode
            {
                CollegeApplicationCycleId = Guid.NewGuid(),
                SpecialCode = _faker.Random.String2(2),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'College Application Cycle Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodeValidator_ShouldFail_When_SpecialCode_Empty()
        {
            // Arrange
            var message = new GetSpecialCode
            {
                CollegeApplicationCycleId = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles).Id,
                SpecialCode = string.Empty,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Special Code' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodeValidator_ShouldFail_When_SpecialCode_TooLong()
        {
            // Arrange
            var message = new GetSpecialCode
            {
                CollegeApplicationCycleId = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles).Id,
                SpecialCode = _faker.Random.String2(3),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'Special Code' must be 2 characters in length. You entered {message.SpecialCode.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodeValidator_ShouldFail_When_User_Null()
        {
            // Arrange
            var message = new GetSpecialCode
            {
                CollegeApplicationCycleId = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles).Id,
                SpecialCode = _faker.Random.String2(2),
                User = null
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'User' must not be empty.");
        }
    }
}
