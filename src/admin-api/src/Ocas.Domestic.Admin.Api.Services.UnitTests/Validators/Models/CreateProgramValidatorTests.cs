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
    public class CreateProgramValidatorTests
    {
        private readonly CreateProgramValidator _validator;
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;
        private readonly AdminTestFramework.ModelFakerFixture _models;

        public CreateProgramValidatorTests()
        {
            var domesticContextMock = new DomesticContextMock();
            _validator = new CreateProgramValidator(XunitInjectionCollection.LookupsCache, domesticContextMock.Object);
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public void CreateProgramValidator_ShouldPass()
        {
            var program = _models.GetProgram().Generate();
            program.Intakes = _models.GetProgramIntake(program).Generate(3);

            var model = new Faker<CreateProgram>()
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
        public void CreateProgramValidator_ShouldFail_When_User_Empty()
        {
            var program = _models.GetProgram().Generate();
            program.Intakes = _models.GetProgramIntake(program).Generate(3);

            var model = new Faker<CreateProgram>()
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
