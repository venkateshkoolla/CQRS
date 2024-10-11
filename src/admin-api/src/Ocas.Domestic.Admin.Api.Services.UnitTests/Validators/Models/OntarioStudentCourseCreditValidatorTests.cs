using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class OntarioStudentCourseCreditValidatorTests
    {
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly OntarioStudentCourseCreditValidator _validator;

        public OntarioStudentCourseCreditValidatorTests()
        {
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _validator = new OntarioStudentCourseCreditValidator(XunitInjectionCollection.LookupsCache, XunitInjectionCollection.DomesticContext);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditValidator_ShouldPass()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCredit().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditValidator_ShouldFail_When_Id_Emtpy()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCredit().Generate();
            model.Id = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Id' must not be empty.");
        }
    }
}
