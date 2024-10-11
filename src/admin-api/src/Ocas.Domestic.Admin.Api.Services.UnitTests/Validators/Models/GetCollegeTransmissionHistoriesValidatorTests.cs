using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class GetCollegeTransmissionHistoriesValidatorTests
    {
        private readonly GetCollegeTransmissionHistoriesValidator _validator;
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;

        public GetCollegeTransmissionHistoriesValidatorTests()
        {
            _validator = new GetCollegeTransmissionHistoriesValidator();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTransmissionHistoriesValidator_ShouldPass()
        {
            // Arrange
            var model = new Faker<GetCollegeTransmissionHistories>()
                .RuleFor(x => x.User, _user)
                .RuleFor(x => x.ApplicationId, f => f.Random.Guid())
                .RuleFor(x => x.Options, new GetCollegeTransmissionHistoryOptions())
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTransmissionHistoriesValidator_ShouldFail_When_ApplicationId_Empty()
        {
            // Arrange
            var model = new Faker<GetCollegeTransmissionHistories>()
                .RuleFor(x => x.User, _user)
                .RuleFor(x => x.ApplicationId, Guid.Empty)
                .RuleFor(x => x.Options, new GetCollegeTransmissionHistoryOptions())
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }
    }
}
