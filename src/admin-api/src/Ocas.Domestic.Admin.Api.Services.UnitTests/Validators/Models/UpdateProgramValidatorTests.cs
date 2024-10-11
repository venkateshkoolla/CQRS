using System;
using System.Security.Principal;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class UpdateProgramValidatorTests
    {
        private readonly UpdateProgramValidator _validator;
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;

        public UpdateProgramValidatorTests()
        {
            var domesticContextMock = new DomesticContextMock();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _validator = new UpdateProgramValidator(XunitInjectionCollection.LookupsCache, domesticContextMock.Object);
        }

        [Fact]
        [UnitTest("Validators")]
        public void UpdateProgramValidator_ShouldPass()
        {
            var program = _models.GetProgram().Generate();
            program.Intakes = _models.GetProgramIntake(program).Generate(3);

            var model = new Faker<UpdateProgram>()
                    .RuleFor(x => x.ProgramId, program.Id)
                    .RuleFor(x => x.Program, program)
                    .RuleFor(x => x.User, _user)
                    .Generate();

            // Act
            var result = _validator.Validate(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public void UpdateProgramValidator_ShouldFail_When_ProgramId_Empty()
        {
            var program = _models.GetProgram().Generate();
            program.Intakes = _models.GetProgramIntake(program).Generate(3);

            var model = new Faker<UpdateProgram>()
                    .RuleFor(x => x.ProgramId, Guid.Empty)
                    .RuleFor(x => x.Program, program)
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
        public void UpdateProgramValidator_ShouldFail_When_ProgramId_NotMatch_With_Program()
        {
            var program = _models.GetProgram().Generate();
            program.Intakes = _models.GetProgramIntake(program).Generate(3);

            var model = new Faker<UpdateProgram>()
                    .RuleFor(x => x.ProgramId, f => f.Random.Guid())
                    .RuleFor(x => x.Program, program)
                    .RuleFor(x => x.User, _user)
                    .Generate();

            // Act
            var result = _validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Program Id' must be equal to '{program.Id}'.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void UpdateProgramValidator_ShouldFail_When_User_Empty()
        {
            var program = _models.GetProgram().Generate();
            program.Intakes = _models.GetProgramIntake(program).Generate(3);

            var model = new Faker<UpdateProgram>()
                    .RuleFor(x => x.ProgramId, program.Id)
                    .RuleFor(x => x.Program, program)
                    .Generate();

            // Act
            var result = _validator.Validate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'User' must not be empty.");
        }
    }
}
