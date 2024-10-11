using System;
using System.Security.Principal;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class DeleteProgramValidatorTests
    {
        private readonly DeleteProgramValidator _validator;
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;

        public DeleteProgramValidatorTests()
        {
            _validator = new DeleteProgramValidator();
        }

        [Fact]
        [UnitTest("Validators")]
        public void DeleteProgramValidator_ShouldPass()
        {
            var model = new Faker<DeleteProgram>()
                    .RuleFor(x => x.ProgramId, f => f.Random.Guid())
                    .RuleFor(x => x.User, _user)
                    .Generate();

            // Act
            var result = _validator.Validate(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public void DeleteProgramValidator_ShouldFail_When_ProgramId_Empty()
        {
            var model = new Faker<DeleteProgram>()
                    .RuleFor(x => x.ProgramId, Guid.Empty)
                    .RuleFor(x => x.User, _user)
                    .Generate();

            // Act
            var result = _validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Program Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void DeleteProgramValidator_ShouldFail_When_User_Empty()
        {
            var model = new Faker<DeleteProgram>()
                    .RuleFor(x => x.ProgramId, f => f.Random.Guid())
                    .Generate();

            // Act
            var result = _validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'User' must not be empty.");
        }
    }
}
