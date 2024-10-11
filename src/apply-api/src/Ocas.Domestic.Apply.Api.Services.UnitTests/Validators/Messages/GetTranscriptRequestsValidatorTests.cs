using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class GetTranscriptRequestsValidatorTests : IClassFixture<GetTranscriptRequestsValidator>
    {
        private readonly GetTranscriptRequestsValidator _validator;
        private readonly IPrincipal _user;

        public GetTranscriptRequestsValidatorTests(GetTranscriptRequestsValidator validator)
        {
            _validator = validator;
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetTranscriptRequestsValidator_ShouldPass_WhenApplicationId()
        {
            // Arrange
            var model = new Faker<GetTranscriptRequests>()
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetTranscriptRequestsValidator_ShouldPass_WhenApplicantId()
        {
            // Arrange
            var model = new Faker<GetTranscriptRequests>()
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
        public async Task GetTranscriptRequestsValidator_ShouldFail_WhenIdsEmpty()
        {
            // Arrange
            var model = new Faker<GetTranscriptRequests>()
                .RuleFor(o => o.ApplicationId, _ => Guid.Empty)
                .RuleFor(o => o.ApplicantId, _ => Guid.Empty)
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty()
                .And.ContainSingle(x => x.ErrorMessage == "'Applicant Id' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }
    }
}