using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class GetApplicantHistoriesValidatorTests
    {
        private readonly GetApplicantHistoriesValidator _validator;
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;

        public GetApplicantHistoriesValidatorTests()
        {
            _validator = new GetApplicantHistoriesValidator();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoriesValidator_ShouldPass()
        {
            // Arrange
            var model = new Faker<GetApplicantHistories>()
                                    .RuleFor(x => x.ApplicantId, Guid.NewGuid())
                                    .RuleFor(x => x.User, _user)
                                    .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetApplicantHistoriesValidator_ShouldFail_When_ApplicationId_Empty()
        {
            // Arrange
            var model = new Faker<GetApplicantHistories>()
                                    .RuleFor(x => x.ApplicantId, Guid.NewGuid())
                                    .RuleFor(x => x.ApplicationId, Guid.Empty)
                                    .RuleFor(x => x.User, _user)
                                    .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }
    }
}
