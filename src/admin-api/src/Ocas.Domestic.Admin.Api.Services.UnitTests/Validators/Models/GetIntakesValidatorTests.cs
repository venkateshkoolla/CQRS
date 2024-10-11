using System;
using System.Security.Principal;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class GetIntakesValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly IUserAuthorization _userAuthorization = Mock.Of<IUserAuthorization>();
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _modelFaker;

        public GetIntakesValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldPass()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles).Id)
                .RuleFor(x => x.CollegeId, f => f.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id)
                .RuleFor(x => x.User, _user)
                .Generate();

            var validator = new GetIntakesValidator(_lookupsCache, _userAuthorization);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldPass_When_OcasUser_And_CollegeId_Null()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles).Id)
                .RuleFor(x => x.CollegeId, _ => null)
                .RuleFor(x => x.User, _user)
                .Generate();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var validator = new GetIntakesValidator(_lookupsCache, userAuthorization.Object);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_ApplicationCycleId_Empty()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, Guid.Empty)
                .RuleFor(x => x.CollegeId, f => f.Random.Guid())
                .RuleFor(x => x.User, _user)
                .Generate();

            var validator = new GetIntakesValidator(_lookupsCache, _userAuthorization);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Application Cycle Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_ApplicationCycleId_NotFound()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, Guid.NewGuid())
                .RuleFor(x => x.CollegeId, f => f.Random.Guid())
                .RuleFor(x => x.User, _user)
                .Generate();

            var validator = new GetIntakesValidator(_lookupsCache, _userAuthorization);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Application Cycle Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_NotOcasUser_And_CollegeId_Empty()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.Random.Guid())
                .RuleFor(x => x.CollegeId, Guid.Empty)
                .RuleFor(x => x.User, _user)
                .Generate();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var validator = new GetIntakesValidator(_lookupsCache, userAuthorization.Object);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'College Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_NotOcasUser_And_CollegeId_Null()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.Random.Guid())
                .RuleFor(x => x.CollegeId, Guid.Empty)
                .RuleFor(x => x.User, _user)
                .Generate();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var validator = new GetIntakesValidator(_lookupsCache, userAuthorization.Object);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'College Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_NotOcasUser_And_CollegeId_NotFound()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.Random.Guid())
                .RuleFor(x => x.CollegeId, f => f.Random.Guid())
                .RuleFor(x => x.User, _user)
                .Generate();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var validator = new GetIntakesValidator(_lookupsCache, userAuthorization.Object);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'College Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_OcasUser_And_CollegeId_Empty()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.Random.Guid())
                .RuleFor(x => x.CollegeId, Guid.Empty)
                .RuleFor(x => x.User, _user)
                .Generate();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var validator = new GetIntakesValidator(_lookupsCache, userAuthorization.Object);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'College Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_OcasUser_And_CollegeId_NotFound()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.Random.Guid())
                .RuleFor(x => x.CollegeId, f => f.Random.Guid())
                .RuleFor(x => x.User, _user)
                .Generate();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var validator = new GetIntakesValidator(_lookupsCache, userAuthorization.Object);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'College Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void GetIntakesValidator_ShouldThrow_When_User_Empty()
        {
            // Arrange
            var model = new Faker<GetIntakes>()
                .RuleFor(x => x.ApplicationCycleId, f => f.Random.Guid())
                .RuleFor(x => x.CollegeId, f => f.Random.Guid())
                .Generate();

            var validator = new GetIntakesValidator(_lookupsCache, _userAuthorization);

            // Act
            var result = validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'User' must not be empty.");
        }
    }
}
