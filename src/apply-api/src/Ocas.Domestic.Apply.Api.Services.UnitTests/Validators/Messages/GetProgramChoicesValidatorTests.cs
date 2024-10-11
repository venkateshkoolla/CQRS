using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Apply.Services.Validators.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class GetProgramChoicesValidatorTests : IClassFixture<GetProgramChoicesValidator>
    {
        private readonly GetProgramChoicesValidator _validator;
        private readonly IPrincipal _user;

        public GetProgramChoicesValidatorTests(GetProgramChoicesValidator validator)
        {
            _validator = validator;
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetProgramChoicesValidator_ShouldPass_When_ApplicantId()
        {
            // Arrange
            var model = new Faker<GetProgramChoices>()
                .RuleFor(o => o.ApplicationId, _ => null)
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetProgramChoicesValidator_ShouldPass_When_ApplicationId()
        {
            // Arrange
            var model = new Faker<GetProgramChoices>()
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicantId, _ => null)
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetProgramChoicesValidator_ShouldFail_When_Ids_Empty()
        {
            // Arrange
            var model = new Faker<GetProgramChoices>()
                .RuleFor(o => o.ApplicationId, _ => Guid.Empty)
                .RuleFor(o => o.ApplicantId, _ => Guid.Empty)
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2)
                .And.ContainSingle(x => x.ErrorMessage == "'Application Id' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
        }
    }
}
