using System;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class DeleteOntarioStudentCourseCreditValidatorTests : IClassFixture<DeleteOntarioStudentCourseCreditValidator>
    {
        private readonly DeleteOntarioStudentCourseCreditValidator _validator;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();

        public DeleteOntarioStudentCourseCreditValidatorTests(DeleteOntarioStudentCourseCreditValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task DeleteOntarioStudentCourseCreditValidator_ShouldPass()
        {
            // Arrange
            var model = new DeleteOntarioStudentCourseCredit
            {
                OntarioStudentCourseCreditId = Guid.NewGuid(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task DeleteOntarioStudentCourseCreditValidator_ShouldFail_When_OntarioStudentCourseCreditId_Empty()
        {
            // Arrange
            var model = new DeleteOntarioStudentCourseCredit
            {
                OntarioStudentCourseCreditId = Guid.Empty,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Ontario Student Course Credit Id' must not be empty.");
        }
    }
}
