using System;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class GetIntakeApplicantsValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly GetIntakeApplicantsValidator _validator;

        public GetIntakeApplicantsValidatorTests()
        {
            _validator = new GetIntakeApplicantsValidator();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramBriefsValidator_ShouldPass()
        {
            // Arrage
            var message = new GetIntakeApplicants
            {
                IntakeId = Guid.NewGuid(),
                Params = new GetIntakeApplicantOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramBriefsValidator_ShouldFail_When_IntakeId_Empty()
        {
            // Arrage
            var message = new GetIntakeApplicants
            {
                IntakeId = Guid.Empty,
                Params = new GetIntakeApplicantOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(message);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Intake Id' must not be empty.");
        }
    }
}
