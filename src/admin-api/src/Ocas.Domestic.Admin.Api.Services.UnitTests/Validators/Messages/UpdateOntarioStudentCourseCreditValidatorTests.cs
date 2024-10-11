using System;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class UpdateOntarioStudentCourseCreditValidatorTests
    {
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly UpdateOntarioStudentCourseCreditValidator _validator;

        public UpdateOntarioStudentCourseCreditValidatorTests()
        {
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _validator = new UpdateOntarioStudentCourseCreditValidator(XunitInjectionCollection.LookupsCache, XunitInjectionCollection.DomesticContext);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task UpdateOntarioStudentCourseCreditValidator_ShouldPass()
        {
            // Arrange
            var courseCredit = _modelFaker.GetOntarioStudentCourseCredit().Generate();
            var model = new UpdateOntarioStudentCourseCredit
            {
                OntarioStudentCourseCredit = courseCredit,
                OntarioStudentCourseCreditId = courseCredit.Id,
                ApplicantId = courseCredit.ApplicantId,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task UpdateOntarioStudentCourseCreditValidator_ShouldFail_When_OntarioStudentCourseCreditId_Empty()
        {
            // Arrange
            var courseCredit = _modelFaker.GetOntarioStudentCourseCredit().Generate();
            var model = new UpdateOntarioStudentCourseCredit
            {
                OntarioStudentCourseCredit = courseCredit,
                OntarioStudentCourseCreditId = Guid.Empty,
                ApplicantId = courseCredit.ApplicantId,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Ontario Student Course Credit Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task UpdateOntarioStudentCourseCreditValidator_ShouldFail_When_OntarioStudentCourseCreditId_DoesNotMatch()
        {
            // Arrange
            var courseCredit = _modelFaker.GetOntarioStudentCourseCredit().Generate();
            var model = new UpdateOntarioStudentCourseCredit
            {
                OntarioStudentCourseCredit = courseCredit,
                OntarioStudentCourseCreditId = Guid.NewGuid(),
                ApplicantId = courseCredit.ApplicantId,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == $"'Ontario Student Course Credit Id' must be equal to '{courseCredit.Id}'.");
        }
    }
}
