using System;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class GetApplicantValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantValidator_ShouldPass()
        {
            // Arrange
            var message = new GetApplicant
            {
                User = _user
            };

            var validator = new GetApplicantValidator();

            // Act
            var result = await validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantValidator_ShouldPass_When_ApplicantId_HasValue()
        {
            // Arrange
            var message = new GetApplicant
            {
                User = _user,
                ApplicantId = Guid.NewGuid()
            };

            var validator = new GetApplicantValidator();

            // Act
            var result = await validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantValidator_ShouldFail_When_ApplicantId_Empty()
        {
            // Arrange
            var message = new GetApplicant
            {
                ApplicantId = Guid.Empty,
                User = _user
            };

            var validator = new GetApplicantValidator();

            // Act
            var result = await validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantValidator_ShouldFail_When_User_Empty()
        {
            // Arrange
            var message = new GetApplicant
            {
                ApplicantId = Guid.NewGuid(),
                User = null
            };

            var validator = new GetApplicantValidator();

            // Act
            var result = await validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'User' must not be empty.");
        }
    }
}
