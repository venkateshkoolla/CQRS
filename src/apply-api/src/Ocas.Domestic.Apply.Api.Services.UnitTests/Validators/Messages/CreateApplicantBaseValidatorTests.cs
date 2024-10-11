using System;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class CreateApplicantBaseValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly ModelFakerFixture _modelFakerFixture;

        public CreateApplicantBaseValidatorTests()
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicantBaseValidator_ShouldPass()
        {
            // Arrange
            var message = new CreateApplicantBase
            {
                ApplicantBase = _modelFakerFixture.GetApplicantBase().Generate(),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var validator = new CreateApplicantBaseValidator(userAuthorization.Object);

            // Act
            var result = await validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicantBaseValidator_ShouldFail_When_ApplicantBase_Null()
        {
            // Arrange
            var message = new CreateApplicantBase
            {
                ApplicantBase = null,
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var validator = new CreateApplicantBaseValidator(userAuthorization.Object);

            // Act
            var result = await validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Applicant Base' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void CreateApplicantBaseValidator_ShouldFail_When_User_IsOcasUser()
        {
            // Arrange
            var message = new CreateApplicantBase
            {
                ApplicantBase = _modelFakerFixture.GetApplicantBase().Generate(),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var validator = new CreateApplicantBaseValidator(userAuthorization.Object);

            // Act
            Func<Task> action = () => validator.ValidateAsync(message);

            // Assert
            action.Should().Throw<ForbiddenException>()
                .WithMessage("'User' must not be an OCAS user.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicantBaseValidator_ShouldFail_When_User_Null()
        {
            // Arrange
            var message = new CreateApplicantBase
            {
                ApplicantBase = _modelFakerFixture.GetApplicantBase().Generate(),
                User = null
            };

            var userAuthorization = Mock.Of<IUserAuthorization>();

            var validator = new CreateApplicantBaseValidator(userAuthorization);

            // Act
            var result = await validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'User' must not be empty.");
        }
    }
}
