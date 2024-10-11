using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class GetSpecialCodesValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly Faker _faker;
        private readonly GetSpecialCodesValidator _validator;

        public GetSpecialCodesValidatorTests()
        {
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _validator = new GetSpecialCodesValidator(XunitInjectionCollection.LookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodesValidator_ShouldPass()
        {
            // Arrange
            var message = new GetSpecialCodes
            {
                CollegeApplicationCycleId = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles).Id,
                Params = new GetSpecialCodeOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetSpecialCodesValidator_ShouldFail_When_CollegeApplicationCycleId_Empty()
        {
            // Arrange
            var message = new GetSpecialCodes
            {
                CollegeApplicationCycleId = Guid.Empty,
                Params = new GetSpecialCodeOptions(),
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
        public async Task GetSpecialCodesValidator_ShouldFail_When_CollegeApplicationCycleId_NotFound()
        {
            // Arrange
            var message = new GetSpecialCodes
            {
                CollegeApplicationCycleId = Guid.NewGuid(),
                Params = new GetSpecialCodeOptions(),
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
        public async Task GetSpecialCodesValidator_ShouldFail_When_SearchParam_TooLong()
        {
            // Arrange
            var message = new GetSpecialCodes
            {
                CollegeApplicationCycleId = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles).Id,
                Params = new GetSpecialCodeOptions
                {
                    Search = _faker.Random.Words(15)
                },
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"The length of 'Params. Search' must be 100 characters or fewer. You entered {message.Params.Search.Length} characters.");
        }
    }
}
