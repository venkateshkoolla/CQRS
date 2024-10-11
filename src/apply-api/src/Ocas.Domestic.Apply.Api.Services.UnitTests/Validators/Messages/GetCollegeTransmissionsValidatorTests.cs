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
    public class GetCollegeTransmissionsValidatorTests : IClassFixture<GetCollegeTransmissionsValidator>
    {
        private readonly GetCollegeTransmissionsValidator _validator;

        public GetCollegeTransmissionsValidatorTests(GetCollegeTransmissionsValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTransmissionsValidator_ShouldPass()
        {
            var message = new Faker<GetCollegeTransmissions>()
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(o => o.User, _ => Mock.Of<IPrincipal>())
                .Generate();

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTransmissionsValidator_ShouldFail_When_ApplicationIdEmpty()
        {
            var message = new Faker<GetCollegeTransmissions>()
                .RuleFor(o => o.ApplicationId, _ => Guid.Empty)
                .RuleFor(o => o.User, _ => Mock.Of<IPrincipal>())
                .Generate();

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }
    }
}
