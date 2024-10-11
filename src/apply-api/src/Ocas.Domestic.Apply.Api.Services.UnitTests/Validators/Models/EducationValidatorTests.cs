using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class EducationValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly EducationValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public EducationValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new EducationValidator(_lookupsCache, new DomesticContextMock().Object);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_WhenInternational()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducation();
            var model = faker.Generate("default,Intl");

            // Act
            var result = await _validator.ValidateAsync(model);
            var educationType = model.GetEducationType(_modelFakerFixture.AllApplyLookups.Countries, _modelFakerFixture.AllApplyLookups.InstituteTypes);

            // Assert
            educationType.Should().Be(EducationType.International);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenMissingId()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducation();
            var model = faker.Generate("default,Intl");
            model.Id = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Id' must not be empty.");
        }
    }
}
